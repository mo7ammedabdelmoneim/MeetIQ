namespace MeetIQ.Application.Interfaces.Services
{
    public interface ITranscriptionService
    {
        /// Transcribes the audio file at the given absolute path.
        /// Returns the transcript text, or throws on failure.
        Task<TranscriptionResult> TranscribeAsync(string audioFilePath, CancellationToken ct = default);
    }

    public record TranscriptionResult(
        string Text,
        string Language,
        double DurationSeconds
    );
}