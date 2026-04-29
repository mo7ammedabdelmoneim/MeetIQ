namespace MeetIQ.Domain.Entities
{
    public class MeetingParticipant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!; 
    }

   
}
