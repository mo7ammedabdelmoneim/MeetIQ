using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserPendingInvitationsQuery;

public class GetUserPendingInvitationsQuery
    : IRequest<List<MeetingInvitationDto>>
{
    public string UserId { get; set; } = string.Empty;
}