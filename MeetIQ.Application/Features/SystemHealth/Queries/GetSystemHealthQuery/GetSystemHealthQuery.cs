using MediatR;
using MeetIQ.Application.Features.SystemHealth.DTOs;

namespace MeetIQ.Application.Features.SystemHealth.Queries.GetSystemHealthQuery
{
    public class GetSystemHealthQuery : IRequest<SystemHealthDto>
    {
    }
}