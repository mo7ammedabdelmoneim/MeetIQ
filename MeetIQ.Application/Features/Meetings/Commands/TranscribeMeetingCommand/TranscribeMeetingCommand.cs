using MediatR;

namespace MeetIQ.Application.Features.Transcripts.Commands.TranscribeMeetingCommand
{
    public class TranscribeMeetingCommand : IRequest<TranscribeMeetingResult>
    {
        public Guid MeetingId { get; set; }
        public string RequestedByUserId { get; set; } = string.Empty;
    }

    public record TranscribeMeetingResult(
        bool Success,
        string? Text,
        string? Error
    );
}