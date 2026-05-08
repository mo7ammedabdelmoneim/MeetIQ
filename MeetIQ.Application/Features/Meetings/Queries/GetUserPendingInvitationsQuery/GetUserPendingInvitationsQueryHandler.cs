using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserPendingInvitationsQuery;

public class GetUserPendingInvitationsQueryHandler
    : IRequestHandler<GetUserPendingInvitationsQuery, List<MeetingInvitationDto>>
{
    private readonly IMeetingRepository meetingRepository;

    public GetUserPendingInvitationsQueryHandler(
        IMeetingRepository meetingRepository)
    {
        this.meetingRepository = meetingRepository;
    }

    public async Task<List<MeetingInvitationDto>> Handle(
        GetUserPendingInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        return await meetingRepository
            .GetUserPendingInvitationsAsync(request.UserId);
    }
}