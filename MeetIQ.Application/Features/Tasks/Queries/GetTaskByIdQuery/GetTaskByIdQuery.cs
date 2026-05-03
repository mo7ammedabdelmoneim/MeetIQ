using MediatR;
using MeetIQ.Application.Features.Tasks.DTOs;

namespace MeetIQ.Application.Features.Tasks.Queries.GetTaskByIdQuery
{
    public class GetTaskByIdQuery : IRequest<TaskDetailsDto?>
    {
        public Guid Id { get; set; }
    }
}
