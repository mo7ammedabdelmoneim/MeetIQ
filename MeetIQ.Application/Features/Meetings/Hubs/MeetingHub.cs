using Microsoft.AspNetCore.SignalR;

namespace MeetIQ.Application.Features.Meetings.Hubs
{
    public class MeetingHub : Hub
    {
        public async Task JoinMeeting(string meetingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"meeting-{meetingId}");
        }
    }
}
