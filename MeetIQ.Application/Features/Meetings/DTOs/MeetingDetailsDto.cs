using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.DTOs
{
    public class MeetingDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string JitsiRoomId { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public MeetingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string HostId { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string HostEmail { get; set; } = string.Empty;
        public string? HostAvatarUrl { get; set; }
        public int ParticipantsCount { get; set; }
        public bool HasTranscript { get; set; }
        public bool HasSummary { get; set; }
        public Guid? TranscriptId { get; set; }
        public Guid? SummaryId { get; set; }
        public List<MeetingParticipantDto> Participants { get; set; } = [];
        public List<MeetingInvitationDto> Invitations { get; set; } = [];

        public TimeSpan? Duration => StartedAt.HasValue && EndedAt.HasValue
            ? EndedAt.Value - StartedAt.Value
            : null;
    }
}