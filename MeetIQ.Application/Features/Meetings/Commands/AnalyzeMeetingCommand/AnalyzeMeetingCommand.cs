using MediatR;

namespace MeetIQ.Application.Features.Meetings.Commands.AnalyzeMeetingCommand
{
    public class AnalyzeMeetingCommand : IRequest<AnalyzeMeetingResult>
    {
        public Guid MeetingId { get; set; }
        public string RequestedByUserId { get; set; } = string.Empty;
    }

    public record AnalyzeMeetingResult(bool Success, string? Error);
}