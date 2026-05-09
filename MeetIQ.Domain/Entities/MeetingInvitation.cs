using MeetIQ.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetIQ.Domain.Entities
{
    public class MeetingInvitation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

        public string InvitedUserId { get; set; } = string.Empty;
        [ForeignKey(nameof(InvitedUserId))]           
        public ApplicationUser InvitedUser { get; set; } = null!;

        public string InvitedByUserId { get; set; } = string.Empty;
        [ForeignKey(nameof(InvitedByUserId))]         
        public ApplicationUser InvitedBy { get; set; } = null!;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime InvitedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
    }
}