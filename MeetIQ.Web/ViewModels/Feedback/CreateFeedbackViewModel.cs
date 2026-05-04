using System.ComponentModel.DataAnnotations;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Web.ViewModels.Feedback
{
    public class CreateFeedbackViewModel
    {
        [Required(ErrorMessage = "Please select a feedback type")]
        public FeedbackType Type { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
        [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters")]
        [Display(Name = "Your Message")]
        public string Message { get; set; } = string.Empty;
    }
}