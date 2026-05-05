using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MeetIQ.Web.ViewModels.Profile
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [MinLength(2), MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Profile Photo")]
        public IFormFile? AvatarFile { get; set; }

        public bool RemoveAvatar { get; set; }


        // Read-only display fields (not submitted)
        public string? CurrentAvatarUrl { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}