namespace MeetIQ.Application.Interfaces.Services
{
    public interface IJitsiTokenService
    {
        string GenerateToken(string roomId, string userId, string userName, string userEmail, bool isModerator);
    }
}