using Azure.Core;
using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery;
using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence;
using MeetIQ.Infrastructure.Presistence.Repositories;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Repositories
{
    public class TranscriptRepository :Repository<MeetingTranscript > , ITranscriptRepository
    {
        public TranscriptRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }

        public async Task<TranscriptDetailsDto?> GetByIdAsync(Guid transcriptId)
        {
            var transcript = await db.Query("MeetingTranscripts as t")
                .Join("Meetings as m", "m.Id", "t.MeetingId")
                .Select(
                    "t.Id",
                    "t.MeetingId",
                    "t.Text",
                    "t.Language",
                    "t.AudioFilePath",
                    "t.Status",
                    "t.CreatedAt",
                    "t.UpdatedAt",
                    "m.Title as MeetingTitle",
                    "m.HostId"
                )
                .Where("t.Id", transcriptId)
                .FirstOrDefaultAsync<TranscriptDetailsDto>();

            if (transcript == null)
                return null;

            transcript.ParticipantIds = (await db.Query("MeetingParticipants")
                .Where("MeetingId", transcript.MeetingId)
                .Select("UserId")
                .GetAsync<string>())
                .ToList();

            return transcript;
        }

        public async Task<List<TranscriptListItemDto>> GetUserTranscriptsAsync(string userId)
        {
            var items = await db.Query("MeetingTranscripts as t")
                .Join("Meetings as m", "m.Id", "t.MeetingId")
                .Select(
                    "t.Id", "t.MeetingId", "t.Status",
                    "t.Language", "t.CreatedAt", "t.UpdatedAt",
                    "t.Text",
                    "m.Title as MeetingTitle"
            )
                .Where("m.HostId", userId)
                .OrderByDesc("t.CreatedAt")
                .GetAsync<TranscriptListItemDto>();

            return items.ToList();
        }
    }
}