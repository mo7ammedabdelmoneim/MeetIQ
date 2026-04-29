using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Commands.DeleteTaskCommand
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository repo;

        public DeleteTaskCommandHandler(ITaskRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await repo.GetAsync(x => x.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException("Task not found");

            // already deleted
            if (task.IsDeleted)
                throw new BadRequestException("Task already deleted");

            // optional rule
            if (task.Status == Domain.Enums.TaskStatus.Done)
                throw new BadRequestException("Cannot delete completed task");

            // Soft delete
            task.IsDeleted = true;
            task.DeletedAt = DateTime.UtcNow;

            repo.Update(task);
            await repo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
