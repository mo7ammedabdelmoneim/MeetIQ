using MeetIQ.Application.Features.SystemHealth.DTOs;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ISystemHealthRepository
    {
        Task<SystemHealthDto> GetSystemHealthAsync();
    }
}