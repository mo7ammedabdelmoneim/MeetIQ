using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IMeetingRepository:IRepository<Meeting>
    {
        Task<List<MeetingListItemDto>> GetUserMeetingsAsync(GetUserMeetingsQuery query);
    }
}
