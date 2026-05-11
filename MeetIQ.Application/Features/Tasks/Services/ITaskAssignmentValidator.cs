namespace MeetIQ.Application.Features.Tasks.Services
{
    public interface ITaskAssignmentValidator
    {
        /// Validates that the email belongs to a registered user
        /// who is allowed to be assigned tasks in the given meeting context.
        /// Returns the userId if valid, throws otherwise.
        Task<string> ValidateAndResolveAsync(string assigneeEmail, Guid? meetingId, string requesterId);
    }
}