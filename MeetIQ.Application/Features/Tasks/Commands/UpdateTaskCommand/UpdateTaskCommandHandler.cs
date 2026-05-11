using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Tasks.Services;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository taskRepository;
        private readonly ITaskAssignmentValidator assignmentValidator;

        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            ITaskAssignmentValidator assignmentValidator)
        {
            this.taskRepository = taskRepository;
            this.assignmentValidator = assignmentValidator;
        }

        public async Task<bool> Handle(
            UpdateTaskCommand request,
            CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetAsync(
                x => x.Id == request.TaskId && !x.IsDeleted);

            if (task == null)
                throw new NotFoundException("Task not found");

            // Only creator or assignee can update
            if (task.UserId != request.RequesterId &&
                task.AssigneeId != request.RequesterId)
                throw new UnauthorizedException("You don't have permission to update this task");

            task.Title = request.Title;
            task.Description = request.Description;
            task.Priority = request.Priority;
            task.Status = request.Status;
            task.DueDate = request.DueDate;

            // Assignee 
            if (request.ClearAssignee)
            {
                task.AssigneeId = null;
            }
            else if (!string.IsNullOrEmpty(request.AssigneeEmail))
            {
                task.AssigneeId = await assignmentValidator.ValidateAndResolveAsync(
                    request.AssigneeEmail,
                    task.MeetingId,
                    request.RequesterId);
            }

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            return true;
        }
    }
}