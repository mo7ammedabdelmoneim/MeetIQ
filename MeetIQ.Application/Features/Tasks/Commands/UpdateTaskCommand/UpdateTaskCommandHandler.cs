using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            this.taskRepository = taskRepository;
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetAsync(x => x.Id == request.Id);

            if (task == null)
                throw new NotFoundException("Task not found");

            // Partial update
            if (request.Title != null)
                task.Title = request.Title;

            if (request.Description is not null)
                task.Description = string.IsNullOrWhiteSpace(request.Description) ? null: request.Description;

            if (request.Priority.HasValue)
                task.Priority = request.Priority.Value;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate;
            
            if (request.MeetingId.HasValue)
                task.MeetingId = request.MeetingId;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
