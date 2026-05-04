namespace MeetIQ.Application.Features.Users.DTOs
{
    public class UserDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = string.Empty;
        public int MeetingsCount { get; set; }
        public int TasksCount { get; set; }
        public int NotesCount { get; set; }
        public int FeedbackCount { get; set; }
        public DateTime? LastMeetingAt { get; set; }
    }
}