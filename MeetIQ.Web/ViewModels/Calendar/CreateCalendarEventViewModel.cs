using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MeetIQ.Web.ViewModels.Calendar
{
    public class CreateCalendarEventViewModel
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required, Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required, Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Required, RegularExpression(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$",
            ErrorMessage = "Must be a valid hex color")]
        public string Color { get; set; } = "#3B82F6";

        [Display(Name = "Link to Meeting (optional)")]
        public Guid? MeetingId { get; set; }

        public List<SelectListItem> Meetings { get; set; } = new();
    }
}