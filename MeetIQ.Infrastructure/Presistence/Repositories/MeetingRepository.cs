using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Features.Meetings.Queries.GetMeetingsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using SqlKata.Execution;
using Microsoft.EntityFrameworkCore;
using MeetIQ.Application.Features.Notifications.Job.DTOs;

namespace MeetIQ.Infrastructure.Persistence.Repositories
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db)
        {
        }

        public async Task<MeetingDetailsDto?> GetByIdAsync(Guid id)
        {
            var meeting = await db.Query("Meetings as m")
                .Join("AspNetUsers as u", "u.Id", "m.HostId")
                .LeftJoin("MeetingTranscripts as tr", "tr.MeetingId", "m.Id")
                .LeftJoin("MeetingSummaries as sm", "sm.MeetingId", "m.Id")
                .Select(
                    "m.Id", "m.Title", "m.JitsiRoomId", "m.ScheduledAt",
                    "m.StartedAt", "m.EndedAt", "m.Status", "m.CreatedAt",
                    "m.HostId",
                    "u.FullName as HostName",
                    "u.Email as HostEmail",
                    "u.AvatarUrl as HostAvatarUrl",
                    "tr.Id as TranscriptId",
                    "sm.Id as SummaryId"
                )
                .SelectRaw("CASE WHEN tr.Id IS NOT NULL THEN 1 ELSE 0 END as HasTranscript")
                .SelectRaw("CASE WHEN sm.Id IS NOT NULL THEN 1 ELSE 0 END as HasSummary")
                .Where("m.Id", id)
                .FirstOrDefaultAsync<MeetingDetailsDto>();

            if (meeting == null) return null;

            // Participants
            meeting.Participants = (await db.Query("MeetingParticipants as mp")
                .Join("AspNetUsers as u", "u.Id", "mp.UserId")
                .Select(
                    "mp.Id", "mp.UserId", "mp.JoinedAt", "mp.LeftAt",
                    "u.FullName", "u.Email", "u.AvatarUrl"
                )
                .Where("mp.MeetingId", id)
                .OrderBy("mp.JoinedAt")
                .GetAsync<MeetingParticipantDto>())
                .ToList();

            meeting.ParticipantsCount = meeting.Participants.Count;

            meeting.Invitations = (await db.Query("MeetingInvitations as mi")
                .Join("AspNetUsers as u", "u.Id", "mi.InvitedUserId")
                .Join("AspNetUsers as ib", "ib.Id", "mi.InvitedByUserId")
                .Select(
                    "mi.Id", "mi.MeetingId", "mi.InvitedUserId",
                    "mi.Status", "mi.InvitedAt", "mi.RespondedAt",
                    "u.FullName as InvitedUserName",
                    "u.Email as InvitedUserEmail",
                    "u.AvatarUrl as InvitedUserAvatarUrl",
                    "ib.FullName as InvitedByName"
                )
                .Where("mi.MeetingId", id)
                .OrderBy("mi.InvitedAt")
                .GetAsync<MeetingInvitationDto>())
                .ToList();

            return meeting;
        }

        public async Task<PagedResult<MeetingListItemDto>> GetMeetingsAsync(GetMeetingsQuery query)
        {
            var baseQuery = db.Query("Meetings as m")
                .Join("AspNetUsers as u", "u.Id", "m.HostId")
                .LeftJoin("MeetingTranscripts as tr", "tr.MeetingId", "m.Id")
                .LeftJoin("MeetingSummaries as sm", "sm.MeetingId", "m.Id")
                .Where(q => q
                    .Where("m.HostId", query.UserId)
                    .OrWhere(sub => sub
                        .WhereExists(db.Query("MeetingParticipants")
                            .WhereRaw("MeetingId = m.Id")
                            .Where("UserId", query.UserId))
                    )
                );

            if (query.Status.HasValue)
                baseQuery.Where("m.Status", (int)query.Status);

            if (!string.IsNullOrEmpty(query.Search))
                baseQuery.WhereLike("m.Title", $"%{query.Search}%");

            if (query.From.HasValue)
                baseQuery.Where("m.ScheduledAt", ">=", query.From.Value);

            if (query.To.HasValue)
                baseQuery.Where("m.ScheduledAt", "<=", query.To.Value);

            var countQuery = baseQuery.Clone();
            var total = await countQuery.CountAsync<int>();

            var items = await baseQuery
                .OrderByDesc("m.ScheduledAt")
                .ForPage(query.Page, query.PageSize)
                .Select(
                    "m.Id", "m.Title", "m.JitsiRoomId", "m.ScheduledAt",
                    "m.StartedAt", "m.EndedAt", "m.Status",
                    "m.HostId", "u.FullName as HostName"
                )
                .SelectRaw("CASE WHEN tr.Id IS NOT NULL THEN 1 ELSE 0 END as HasTranscript")
                .SelectRaw("CASE WHEN sm.Id IS NOT NULL THEN 1 ELSE 0 END as HasSummary")
                .SelectRaw("(SELECT COUNT(*) FROM MeetingParticipants WHERE MeetingId = m.Id) as ParticipantsCount")
                .GetAsync<MeetingListItemDto>();

            return new PagedResult<MeetingListItemDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<List<MeetingSelectDto>> GetUserMeetingSelectListAsync(string userId)
        {
            var items = await db.Query("Meetings")
                .Where("HostId", userId)
                .WhereIn("Status", new[] { (int)MeetingStatus.Scheduled, (int)MeetingStatus.InProgress })
                .OrderByDesc("ScheduledAt")
                .Select("Id", "Title")
                .GetAsync<MeetingSelectDto>();

            return items.ToList();
        }

        public async Task<MeetingParticipant?> GetParticipantAsync(Guid meetingId, string userId)
        {
            return await context.Set<MeetingParticipant>()
                .FirstOrDefaultAsync(x => x.MeetingId == meetingId && x.UserId == userId);
        }

        public async Task AddParticipantAsync(MeetingParticipant participant)
        {
            await context.Set<MeetingParticipant>().AddAsync(participant);
        }

        public void UpdateParticipant(MeetingParticipant participant)
        {
            context.Set<MeetingParticipant>().Update(participant);
        }


        public async Task MarkAllParticipantsLeftAsync(Guid meetingId, DateTime leftAt)
        {
            await context.Set<MeetingParticipant>()
                .Where(p => p.MeetingId == meetingId && p.LeftAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.LeftAt, leftAt));
        }



        public async Task<List<MeetingStartingDto>> GetMeetingsStartingBetweenAsync(DateTime from, DateTime to)
        {
            // Get meetings
            var meetings = (await db.Query("Meetings as m")
                .WhereBetween("m.ScheduledAt", from, to)
                .Where("m.Status", (int)MeetingStatus.Scheduled)
                //.Where("m.IsDeleted", false)
                .Select("m.Id", "m.Title", "m.HostId", "m.ScheduledAt")
                .GetAsync<MeetingStartingDto>()).ToList();

            if (!meetings.Any()) return meetings;

            // Get participants for each meeting
            var meetingIds = meetings.Select(m => m.Id).ToList();

            var participants = (await db.Query("MeetingParticipants")
                .WhereIn("MeetingId", meetingIds)
                .Select("MeetingId", "UserId")
                .GetAsync()).ToList();

            foreach (var meeting in meetings)
            {
                meeting.ParticipantIds = participants
                    .Where(p => (Guid)p.MeetingId == meeting.Id)
                    .Select(p => (string)p.UserId)
                    .ToList();
            }

            return meetings;
        }

        public async Task<bool> WasNotifiedRecentlyAsync(
            Guid meetingId, NotificationType type, int withinHours)
        {
            var since = DateTime.UtcNow.AddHours(-withinHours);

            var count = await db.Query("Notifications")
                .Where("ReferenceId", meetingId)
                .Where("Type", (int)type)
                .Where("CreatedAt", ">", since)
                //.Where("IsDeleted", false)
                .CountAsync<int>();

            return count > 0;
        }



        public async Task<MeetingInvitation?> GetInvitationAsync(Guid meetingId, string userId)
        {
            return await context.Set<MeetingInvitation>()
                .FirstOrDefaultAsync(x => x.MeetingId == meetingId && x.InvitedUserId == userId);
        }

        public async Task<MeetingInvitation?> GetInvitationByIdAsync(Guid invitationId)
        {
            return await context.Set<MeetingInvitation>()
                .FirstOrDefaultAsync(x => x.Id == invitationId);
        }

        public async Task<List<MeetingInvitationDto>> GetUserPendingInvitationsAsync(string userId)
        {
            var items = await db.Query("MeetingInvitations as mi")
                .Join("Meetings as m", "m.Id", "mi.MeetingId")
                .Join("AspNetUsers as ib", "ib.Id", "mi.InvitedByUserId")
                .Select(
                    "mi.Id", "mi.MeetingId", "mi.InvitedUserId",
                    "mi.Status", "mi.InvitedAt", "mi.RespondedAt",
                    "m.Title as MeetingTitle",
                    "m.ScheduledAt as MeetingScheduledAt",
                    "ib.FullName as InvitedByName"
                )
                .Where("mi.InvitedUserId", userId)
                .Where("mi.Status", (int)Domain.Enums.InvitationStatus.Pending)
                .OrderByDesc("mi.InvitedAt")
                .GetAsync<MeetingInvitationDto>();

            return items.ToList();
        }

        public async Task AddInvitationAsync(MeetingInvitation invitation)
        {
            await context.Set<MeetingInvitation>().AddAsync(invitation);
        }

        public void UpdateInvitation(MeetingInvitation invitation)
        {
            context.Set<MeetingInvitation>().Update(invitation);
        }

        public void DeleteInvitation(MeetingInvitation invitation)
        {
            context.Set<MeetingInvitation>().Remove(invitation);
        }

        public async Task<List<UserSearchResultDto>> SearchUsersToInviteAsync(
            string term, Guid meetingId, string hostId, int limit)
        {
            var alreadyInvited = await db.Query("MeetingInvitations")
                .Where("MeetingId", meetingId)
                .Select("InvitedUserId")
                .GetAsync<string>();

            var excludedIds = alreadyInvited.Append(hostId).ToList();

            var query = db.Query("AspNetUsers")
                .Where("IsActive", true)
                .Where(q => q
                    .WhereLike("FullName", $"%{term}%")
                    .OrWhereLike("Email", $"%{term}%")
                );

            foreach (var id in excludedIds)
                query.WhereNot("Id", id);

            var items = await query
                .Select("Id", "FullName", "Email", "AvatarUrl")
                .Limit(limit)
                .GetAsync<UserSearchResultDto>();

            return items.ToList();
        }
    }
}

