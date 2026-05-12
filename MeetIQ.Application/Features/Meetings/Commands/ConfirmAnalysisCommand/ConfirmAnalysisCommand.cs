using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.ConfirmAnalysisCommand
{
    public class ConfirmAnalysisCommand : ICommand<ConfirmAnalysisResult>
    {
        public Guid MeetingId { get; set; }
        public string RequestedByUserId { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;
        public string KeyInsights { get; set; } = string.Empty;
        public List<string> KeyDecisions { get; set; } = [];

        public List<PreviewTaskDto> ApprovedTasks { get; set; } = [];
        public List<PreviewNoteDto> ApprovedNotes { get; set; } = [];
    }

    public record ConfirmAnalysisResult(bool Success, string? Error);
}