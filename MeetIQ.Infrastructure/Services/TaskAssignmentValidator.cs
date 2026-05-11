using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Tasks.Services;
using MeetIQ.Domain.Enums;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Services
{
    public class TaskAssignmentValidator : ITaskAssignmentValidator
    {
        private readonly QueryFactory db;

        public TaskAssignmentValidator(QueryFactory db)
        {
            this.db = db;
        }

        public async Task<string> ValidateAndResolveAsync(
            string assigneeEmail,
            Guid? meetingId,
            string requesterId)
        {
            var assignee = await db.Query("AspNetUsers")
                .Where("NormalizedEmail", assigneeEmail.ToUpperInvariant())
                .Where("IsActive", true)
                .Select("Id")
                .FirstOrDefaultAsync<dynamic>();

            if (assignee == null)
                throw new BadRequestException("No active user found with this email address.");

            string assigneeId = assignee.Id;

            if (assigneeId == requesterId)
                return assigneeId;

            if (!meetingId.HasValue)
                throw new BadRequestException(
                    "You can only assign tasks to yourself when no meeting is selected.");

            var isAllowed = await IsAllowedInMeetingAsync(assigneeId, meetingId.Value);

            if (!isAllowed)
                throw new BadRequestException(
                    "This user is not a participant or invitee of the selected meeting.");

            return assigneeId;
        }

        private async Task<bool> IsAllowedInMeetingAsync(string userId, Guid meetingId)
        {
            // Host
            var isHost = await db.Query("Meetings")
                .Where("Id", meetingId)
                .Where("HostId", userId)
                .CountAsync<int>() > 0;

            if (isHost) return true;

            // Participant 
            var isParticipant = await db.Query("MeetingParticipants")
                .Where("MeetingId", meetingId)
                .Where("UserId", userId)
                .CountAsync<int>() > 0;

            if (isParticipant) return true;

            // Accepted Invitation
            var isInvited = await db.Query("MeetingInvitations")
                .Where("MeetingId", meetingId)
                .Where("InvitedUserId", userId)
                .Where("Status", (int)InvitationStatus.Accepted)
                .CountAsync<int>() > 0;

            return isInvited;
        }
    }
}