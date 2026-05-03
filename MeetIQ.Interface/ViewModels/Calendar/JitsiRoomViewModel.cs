using MeetIQ.Domain.Entities;

namespace MeetIQ.Interface.ViewModels.Calendar
{
    public class JitsiRoomViewModel
    {
        public Meeting Meeting { get; set; } = null!;
        public string? JwtToken { get; set; }
        public bool IsHost { get; set; }
        public string JitsiDomain { get; set; } = "meet.jit.si";
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
