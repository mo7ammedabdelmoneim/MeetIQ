using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }

        public async Task<List<MeetingListItemDto>> GetUserMeetingsAsync(GetUserMeetingsQuery query)
        {
            var baseQuery = db.Query("Meetings as m")
                .LeftJoin("MeetingParticipants as mp", "m.Id", "mp.MeetingId")
                .Where(q =>
                    q.Where("m.HostId", query.UserId)
                     .OrWhere("mp.UserId", query.UserId)
                );

            // Filtering
            if (query.FromDate.HasValue)
                baseQuery.Where("m.ScheduledAt", ">=", query.FromDate);

            if (query.ToDate.HasValue)
                baseQuery.Where("m.ScheduledAt", "<=", query.ToDate);

            var result = await baseQuery
                .GroupBy("m.Id", "m.Title", "m.ScheduledAt", "m.EndedAt", "m.HostId")
                .Select("m.Id", "m.Title", "m.ScheduledAt", "m.EndedAt")
                .SelectRaw("COUNT(mp.UserId) as ParticipantsCount")
                .SelectRaw(
                    "CASE WHEN m.HostId = ? THEN CAST(1 as bit) ELSE CAST(0 as bit) END as IsOwner",
                    query.UserId
                )
                .OrderByDesc("m.ScheduledAt")
                .GetAsync<MeetingListItemDto>();

            return result.ToList();
        }
    }
}
