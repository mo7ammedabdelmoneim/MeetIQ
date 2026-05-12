using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Features.Meetings.Queries.GetMeetingsQuery;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using MeetIQ.Application.Features.Notifications.Job.DTOs;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IMeetingRepository:IRepository<Meeting>
    {
        Task<MeetingDetailsDto?> GetByIdAsync(Guid id);
        Task<PagedResult<MeetingListItemDto>> GetMeetingsAsync(GetMeetingsQuery query);
        Task<List<MeetingSelectDto>> GetUserMeetingSelectListAsync(string userId);
        Task<MeetingParticipant?> GetParticipantAsync(Guid meetingId, string userId);
        Task AddParticipantAsync(MeetingParticipant participant);
        void UpdateParticipant(MeetingParticipant participant);
        Task MarkAllParticipantsLeftAsync(Guid meetingId, DateTime leftAt);

        Task<List<MeetingStartingDto>> GetMeetingsStartingBetweenAsync(DateTime from, DateTime to);
        Task<bool> WasNotifiedRecentlyAsync(Guid meetingId, NotificationType type, int withinHours);

        Task<MeetingInvitation?> GetInvitationAsync(Guid meetingId, string userId);
        Task<MeetingInvitation?> GetInvitationByIdAsync(Guid invitationId);
        Task<List<MeetingInvitationDto>> GetUserPendingInvitationsAsync(string userId);
        Task AddInvitationAsync(MeetingInvitation invitation);
        void UpdateInvitation(MeetingInvitation invitation);
        void DeleteInvitation(MeetingInvitation invitation);
        Task<List<string>> GetInvitedUserIdsAsync(Guid meetingId);
        Task<List<UserSearchResultDto>> SearchUsersToInviteAsync(string term, Guid meetingId, string hostId, int limit);

        Task<Meeting> GetMeetingWithTranscription(Guid meetingId, CancellationToken ct);

    }
}