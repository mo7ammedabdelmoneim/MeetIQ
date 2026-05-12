namespace MeetIQ.Application.Interfaces.Services
{
    public interface IAnalysisService
    {
        Task<MeetingAnalysisResult> AnalyzeTranscriptAsync(
            string transcript,
            string meetingTitle,
            CancellationToken ct = default);
    }

    public class MeetingAnalysisResult
    {
        public string Summary { get; set; } = string.Empty;
        public string KeyInsights { get; set; } = string.Empty;
        public List<string>? KeyDecisions { get; set; }

        public List<ExtractedTask> Tasks { get; set; } = [];
        public List<ExtractedNote> Notes { get; set; } = [];
    }

    public class ExtractedTask
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";   
        public string? DueDate { get; set; }            
    }

    public class ExtractedNote
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}