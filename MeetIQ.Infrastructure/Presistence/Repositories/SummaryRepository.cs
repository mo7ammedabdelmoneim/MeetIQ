using MeetIQ.Application.Features.Summaries.Queries.GetSummariesQuery;
using MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence;
using MeetIQ.Infrastructure.Presistence.Repositories;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Repositories
{
    public class SummaryRepository : Repository<MeetingSummary>, ISummaryRepository
    {
        public SummaryRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }

        public async Task<SummaryDetailsDto?> GetByIdAsync(Guid summaryId)
        {
            var summary = await db.Query("MeetingSummaries as s")
                .Join("Meetings as m", "m.Id", "s.MeetingId")
                .Select(
                    "s.Id",
                    "s.MeetingId",
                    "s.SummaryText",
                    "s.KeyInsights",
                    "s.KeyDecisions",
                    "s.IsEdited",
                    "s.GeneratedAt",
                    "s.UpdatedAt",
                    "m.Title as MeetingTitle",
                    "m.HostId"
                )
                .Where("s.Id", summaryId)
                .FirstOrDefaultAsync<SummaryDetailsDto>();

            if (summary == null)
                return null;

            summary.ParticipantIds = (await db.Query("MeetingParticipants")
                .Where("MeetingId", summary.MeetingId)
                .Select("UserId")
                .GetAsync<string>())
                .ToList();

            return summary;
        }

        public async Task<List<SummaryListItemDto>> GetUserSummariesAsync(string userId)
        {
            var items = await db.Query("MeetingSummaries as s")
                .Join("Meetings as m", "m.Id", "s.MeetingId")
                .Select(
                    "s.Id",
                    "s.MeetingId",
                    "s.SummaryText",
                    "s.IsEdited",
                    "s.GeneratedAt",
                    "s.UpdatedAt",
                    "m.Title as MeetingTitle"
                )
                .Where("m.HostId", userId)
                .OrderByDesc("s.GeneratedAt")
                .GetAsync<SummaryListItemDto>();

            return items.ToList();
        }
    }
}