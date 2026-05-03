using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Interface.ViewModels.Notes
{
    public class NoteCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public Guid? MeetingId { get; set; }
        public Guid? CalendarEventId { get; set; }

        public string TagsInput { get; set; } = string.Empty;

        public IEnumerable<SelectOption> Meetings { get; set; } = [];
        public IEnumerable<SelectOption> CalendarEvents { get; set; } = [];
        public IEnumerable<string> AllTags { get; set; } = [];
    }
}