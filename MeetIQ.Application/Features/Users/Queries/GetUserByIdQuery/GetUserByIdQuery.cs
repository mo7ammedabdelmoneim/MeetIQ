using MediatR;
using MeetIQ.Application.Features.Users.DTOs;

namespace MeetIQ.Application.Features.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQuery : IRequest<UserDetailsDto?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}