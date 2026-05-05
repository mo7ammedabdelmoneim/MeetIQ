//using MeetIQ.Web.ViewModels.Calendar;
//using System.ComponentModel.DataAnnotations;

//namespace MeetIQ.Web.ViewModels.Mettings
//{
//    public class CreateMeetingViewModel
//    {
//        [Required, StringLength(200, MinimumLength = 3)]
//        [Display(Name = "Meeting Title")]
//        public string Title { get; set; } = string.Empty;

//        [Required]
//        [FutureDate]
//        [Display(Name = "Scheduled At")]
//        public DateTime ScheduledAt { get; set; } = DateTime.Now.AddHours(1);
//    }


//}

using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Web.ViewModels.Meetings
{
    public class CreateMeetingViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3), MaxLength(200)]
        [Display(Name = "Meeting Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Scheduled date is required")]
        [Display(Name = "Scheduled At")]
        public DateTime ScheduledAt { get; set; } = DateTime.Now.AddHours(1);
    }
}
