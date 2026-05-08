using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.DTOs
{
    public class MeetingInvitationDto
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingTitle { get; set; } = string.Empty;
        public string InvitedUserId { get; set; } = string.Empty;
        public string InvitedUserName { get; set; } = string.Empty;
        public string InvitedUserEmail { get; set; } = string.Empty;
        public string? InvitedUserAvatarUrl { get; set; }
        public string InvitedByName { get; set; } = string.Empty;
        public InvitationStatus Status { get; set; }
        public DateTime InvitedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public DateTime MeetingScheduledAt { get; set; }
    }
}