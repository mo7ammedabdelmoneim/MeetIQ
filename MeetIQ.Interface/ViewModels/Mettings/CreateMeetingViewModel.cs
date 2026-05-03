using MeetIQ.Interface.ViewModels.Calendar;
using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Interface.ViewModels.Mettings
{
    public class CreateMeetingViewModel
    {
        [Required, StringLength(200, MinimumLength = 3)]
        [Display(Name = "Meeting Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [FutureDate]
        [Display(Name = "Scheduled At")]
        public DateTime ScheduledAt { get; set; } = DateTime.Now.AddHours(1);
    }


}
