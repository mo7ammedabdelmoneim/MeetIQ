namespace MeetIQ.Application.Features.Meetings.DTOs
{
    public class PreviewTaskDto
    {
        public int TempId { get; set; }   
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";
        public string? DueDate { get; set; }
    }
}