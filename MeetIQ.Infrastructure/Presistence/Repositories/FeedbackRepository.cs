using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Application.Features.Feedback.Queries.GetFeedbacksQuery;
using MeetIQ.Application.Features.Feedback.Queries.GetMyFeedbacksQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Persistence.Repositories
{
    public class FeedbackRepository : Repository<FeedbackReport>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db)
        {
        }

        public async Task<FeedbackDetailsDto?> GetByIdAsync(Guid id)
        {
            var result = await db.Query("FeedbackReports as f")
                .Join("AspNetUsers as u", "u.Id", "f.ReporterId")
                .Select(
                    "f.Id",
                    "f.Type",
                    "f.Message",
                    "f.Status",
                    "f.CreatedAt",
                    "f.ReporterId",
                    "u.FullName as ReporterName",
                    "u.Email as ReporterEmail"
                )
                .Where("f.Id", id)
                .FirstOrDefaultAsync<FeedbackDetailsDto>();

            return result;
        }

        public async Task<PagedResult<FeedbackListItemDto>> GetFeedbacksAsync(GetFeedbacksQuery query)
        {
            var baseQuery = db.Query("FeedbackReports as f")
                .Join("AspNetUsers as u", "u.Id", "f.ReporterId");

            // Filters
            if (query.Status.HasValue)
                baseQuery.Where("f.Status", (int)query.Status);

            if (query.Type.HasValue)
                baseQuery.Where("f.Type", (int)query.Type);

            if (!string.IsNullOrEmpty(query.Search))
                baseQuery.WhereLike("f.Message", $"%{query.Search}%");

            // Count
            var countQuery = baseQuery.Clone();
            var total = await countQuery.CountAsync<int>();

            // Fetch
            var items = await baseQuery
                .OrderByDesc("f.CreatedAt")
                .ForPage(query.Page, query.PageSize)
                .Select(
                    "f.Id",
                    "f.Type",
                    "f.Message",
                    "f.Status",
                    "f.CreatedAt",
                    "u.FullName as ReporterName",
                    "u.Email as ReporterEmail"
                )
                .GetAsync<FeedbackListItemDto>();

            return new PagedResult<FeedbackListItemDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<FeedbackListItemDto>> GetMyFeedbacksAsync(GetMyFeedbacksQuery query)
        {
            var baseQuery = db.Query("FeedbackReports as f")
                .Join("AspNetUsers as u", "u.Id", "f.ReporterId")
                .Where("f.ReporterId", query.UserId);

            var countQuery = baseQuery.Clone();
            var total = await countQuery.CountAsync<int>();

            var items = await baseQuery
                .OrderByDesc("f.CreatedAt")
                .ForPage(query.Page, query.PageSize)
                .Select(
                    "f.Id",
                    "f.Type",
                    "f.Message",
                    "f.Status",
                    "f.CreatedAt",
                    "u.FullName as ReporterName",
                    "u.Email as ReporterEmail"
                )
                .GetAsync<FeedbackListItemDto>();

            return new PagedResult<FeedbackListItemDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<int> GetOpenFeedbacksCount()
        {
            return await db.Query("FeedbackReports")
                .Where("Status", (int)Domain.Enums.FeedbackStatus.Open)
                .CountAsync<int>();
        }
    }
}