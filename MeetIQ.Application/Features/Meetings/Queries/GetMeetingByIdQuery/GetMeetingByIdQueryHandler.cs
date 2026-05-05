using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery
{
    public class GetMeetingByIdQueryHandler : IRequestHandler<GetMeetingByIdQuery, MeetingDetailsDto?>
    {
        private readonly IMeetingRepository meetingRepository;

        public GetMeetingByIdQueryHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<MeetingDetailsDto?> Handle(
            GetMeetingByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await meetingRepository.GetByIdAsync(request.MeetingId);
        }
    }
}