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
    }
}

