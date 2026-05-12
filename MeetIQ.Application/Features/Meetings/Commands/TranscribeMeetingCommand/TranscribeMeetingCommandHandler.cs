using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MeetIQ.Application.Features.Transcripts.Commands.TranscribeMeetingCommand
{
    public class TranscribeMeetingCommandHandler
        : IRequestHandler<TranscribeMeetingCommand, TranscribeMeetingResult>
    {
        private readonly ITranscriptRepository transcriptionRepository;
        private readonly ITranscriptionService _transcription;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TranscribeMeetingCommandHandler> _logger;

        public TranscribeMeetingCommandHandler(
            ITranscriptRepository transcriptionRepository,
            ITranscriptionService transcription,
            IWebHostEnvironment env,
            ILogger<TranscribeMeetingCommandHandler> logger)
        {
            this.transcriptionRepository = transcriptionRepository;
            _transcription = transcription;
            _env = env;
            _logger = logger;
        }

        public async Task<TranscribeMeetingResult> Handle(
            TranscribeMeetingCommand request,
            CancellationToken ct)
        {
            var transcript = await transcriptionRepository.GetAsync(t => t.MeetingId == request.MeetingId);

            if (transcript is null)
                return new(false, null, "No recording found for this meeting.");

            if (string.IsNullOrEmpty(transcript.AudioFilePath))
                return new(false, null, "Audio file path is missing.");

            // Build absolute path from relative wwwroot path
            var absolutePath = Path.Combine(
                _env.WebRootPath,
                transcript.AudioFilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)
            );

            if (!File.Exists(absolutePath))
                return new(false, null, "Audio file not found on disk.");

            transcript.Status = TranscriptStatus.Transcribing;
            transcript.UpdatedAt = DateTime.UtcNow;
            await transcriptionRepository.SaveChangesAsync();

            try
            {
                // Call Whisper
                var result = await _transcription.TranscribeAsync(absolutePath, ct);

                transcript.Text = result.Text;
                transcript.Language = result.Language;
                transcript.Status = TranscriptStatus.Completed;
                transcript.UpdatedAt = DateTime.UtcNow;
                await transcriptionRepository.SaveChangesAsync();

                return new(true, result.Text, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transcription failed for meeting {MeetingId}", request.MeetingId);

                transcript.Status = TranscriptStatus.Failed;
                transcript.UpdatedAt = DateTime.UtcNow;
                await transcriptionRepository.SaveChangesAsync();

                return new(false, null, ex.Message);
            }
        }
    }
}