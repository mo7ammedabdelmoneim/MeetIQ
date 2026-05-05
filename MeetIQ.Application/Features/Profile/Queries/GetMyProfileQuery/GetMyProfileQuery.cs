using MediatR;
using MeetIQ.Application.Features.Profile.DTOs;

namespace MeetIQ.Application.Features.Profile.Queries.GetMyProfileQuery
{
    public class GetMyProfileQuery : IRequest<MyProfileDto?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}