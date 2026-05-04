using MediatR;
using MeetIQ.Application.Features.SystemHealth.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.SystemHealth.Queries.GetSystemHealthQuery
{
    public class GetSystemHealthQueryHandler : IRequestHandler<GetSystemHealthQuery, SystemHealthDto>
    {
        private readonly ISystemHealthRepository systemHealthRepository;

        public GetSystemHealthQueryHandler(ISystemHealthRepository systemHealthRepository)
        {
            this.systemHealthRepository = systemHealthRepository;
        }

        public async Task<SystemHealthDto> Handle(
            GetSystemHealthQuery request,
            CancellationToken cancellationToken)
        {
            return await systemHealthRepository.GetSystemHealthAsync();
        }
    }
}

