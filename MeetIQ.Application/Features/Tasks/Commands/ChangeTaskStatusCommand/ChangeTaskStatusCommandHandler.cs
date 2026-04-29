using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Commands.ChangeTaskStatusCommand
{
    public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, Unit>
    {
        private readonly ITaskRepository taskRepository;

        public ChangeTaskStatusCommandHandler(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetAsync(x => x.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException("Task not found");
            
            task.Status = request.Status;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
