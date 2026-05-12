using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MeetIQ.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using MeetIQ.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Infrastructure.Services
{
    public class RecordingService : IRecordingService
    {
        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext db;
        private readonly ILogger<RecordingService> logger;

        // Recordings are stored at  wwwroot/recordings/<meetingId>.<ext>
        // Make sure the folder exists (created below if missing).
        private const string Folder = "recordings";

        public RecordingService(
            IWebHostEnvironment env,
            ApplicationDbContext db,
            ILogger<RecordingService> logger)
        {
            this.env = env;
            this.db = db;
            this.logger = logger;
        }

        public async Task<string> SaveAsync(
            Guid meetingId,
            IFormFile audio,
            CancellationToken ct = default)
        {
            // 1. Build destination path
            var ext = GetExtension(audio.ContentType, audio.FileName);
            var fileName = $"{meetingId}{ext}";
            var dir = Path.Combine(env.WebRootPath, Folder);
            Directory.CreateDirectory(dir);                        // no-op if exists

            var fullPath = Path.Combine(dir, fileName);
            var relativePath = $"/{Folder}/{fileName}";

            // 2. Stream to disk (avoids loading 100s of MB into memory)
            await using var fs = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81_920,          // 80 KB chunks
                useAsync: true);

            await audio.CopyToAsync(fs, ct);

            logger.LogInformation(
                "Recording saved for meeting {MeetingId}: {Path} ({Size:N0} bytes)",
                meetingId, relativePath, audio.Length);

            // 3. Persist the path on the Meeting entity
            //    MeetingTranscript stores the raw audio path before transcription.
            var transcript = await db.MeetingTranscripts
                .FirstOrDefaultAsync(t => t.MeetingId == meetingId, ct);

            if (transcript == null)
            {
                transcript = new MeetingTranscript
                {
                    MeetingId = meetingId,
                    AudioFilePath = relativePath,
                    CreatedAt = DateTime.UtcNow,
                    Status = TranscriptStatus.PendingTranscription
                };
                db.MeetingTranscripts.Add(transcript);
            }
            else
            {
                // Replace existing recording (re-upload scenario)
                transcript.AudioFilePath = relativePath;
                transcript.Status = TranscriptStatus.PendingTranscription;
                transcript.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(ct);

            return relativePath;
        }

        public async Task<string?> GetPathAsync(Guid meetingId, CancellationToken ct = default)
        {
            var transcript = await db.MeetingTranscripts
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.MeetingId == meetingId, ct);

            if (transcript?.AudioFilePath == null) return null;

            var fullPath = Path.Combine(env.WebRootPath, transcript.AudioFilePath.TrimStart('/'));
            return File.Exists(fullPath) ? fullPath : null;
        }

        // Helpers 

        private static string GetExtension(string contentType, string fileName)
        {
            // Prefer content-type; fall back to file extension from the original name
            return contentType switch
            {
                "audio/webm" => ".webm",
                "audio/ogg" => ".ogg",
                "audio/mp4" or "audio/mpeg" => ".mp4",
                "audio/wav" => ".wav",
                _ => Path.GetExtension(fileName).ToLowerInvariant() is { Length: > 0 } ext
                         ? ext
                         : ".webm"             
            };
        }
    }
}