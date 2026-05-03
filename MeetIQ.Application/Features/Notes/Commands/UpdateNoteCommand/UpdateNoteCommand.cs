using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Notes.Commands.UpdateNoteCommand
{
    public class UpdateNoteCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string RequesterId { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];

        public Guid? MeetingId { get; set; }
        public Guid? CalendarEventId { get; set; }
    }
}