namespace MeetIQ.Application.Features.Meetings.DTOs
{
    public class MeetingListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DateTime ScheduledAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int ParticipantsCount { get; set; }

        public bool IsOwner { get; set; }
    }
}
