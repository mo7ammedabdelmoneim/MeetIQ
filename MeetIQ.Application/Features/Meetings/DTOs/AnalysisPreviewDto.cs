namespace MeetIQ.Application.Features.Meetings.DTOs
{
    public class AnalysisPreviewDto
    {
        public string Summary { get; set; } = string.Empty;
        public string KeyInsights { get; set; } = string.Empty;
        public List<string> KeyDecisions { get; set; } = [];
        public List<PreviewTaskDto> Tasks { get; set; } = [];
        public List<PreviewNoteDto> Notes { get; set; } = [];
    }
}