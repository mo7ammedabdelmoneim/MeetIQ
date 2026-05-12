namespace MeetIQ.Domain.Enums
{
    public enum TranscriptStatus
    {
        PendingTranscription,   // audio uploaded, not yet transcribed
        Transcribing,           // Whisper job running
        Completed,              // text ready
        Failed
    }
}
