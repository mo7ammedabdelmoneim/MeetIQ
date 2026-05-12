using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.PreviewAnalysisCommand
{
    public class PreviewAnalysisCommand : ICommand<PreviewAnalysisResult>
    {
        public Guid MeetingId { get; set; }
        public string RequestedByUserId { get; set; } = string.Empty;
    }

    public record PreviewAnalysisResult(bool Success, string? Error, AnalysisPreviewDto? Preview);
}