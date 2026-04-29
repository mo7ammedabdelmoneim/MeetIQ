using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Queries.GetTaskByIdQuery
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto?>
    {
        private readonly ITaskRepository _repo;

        public GetTaskByIdQueryHandler(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<TaskDetailsDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.Id);

            if (task == null)
                throw new NotFoundException("Task not found");

            return task;
        }
    }
}
