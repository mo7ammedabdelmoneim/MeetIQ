//using MeetIQ.Application.Common.Options;
//using MeetIQ.Application.Common.Results;
//using MeetIQ.Application.Features.Notes.DTOs;
//using MeetIQ.Application.Features.Notes.Queries.GetNotes;
//using MeetIQ.Application.Features.Tasks.DTOs;
//using MeetIQ.Application.Features.Tasks.Queries.GetTasksQuery;
//using MeetIQ.Domain.Entities;
//using MeetingSelectDto = MeetIQ.Application.Features.Notes.DTOs.MeetingSelectDto;

//namespace MeetIQ.Application.Interfaces.Repositories
//{
//    //public interface INoteRepository : IRepository<Note>
//    //{
//    //    Task<IEnumerable<SelectOption>> GetMeetingsAsync();
//    //    Task<IEnumerable<SelectOption>> GetCalendarEventsAsync();
//    //    Task<IEnumerable<string>> GetAllTagsAsync();



//    //}

//    public interface INoteRepository : IRepository<Note>
//    {
//        /// <summary>Dapper/SqlKata — returns paged list cards for the Index view.</summary>
//        Task<PagedResult<NoteListItemDto>> GetNotesAsync(GetAllNotesQuery query);

//        /// <summary>Dapper/SqlKata — returns full detail including tags.</summary>
//        Task<NoteDetailsDto?> GetByIdAsync(Guid id, string userId);

//        /// <summary>SqlKata — returns tag names already used by this user.</summary>
//        Task<List<string>> GetUserTagsAsync(string userId);

//        /// <summary>SqlKata — returns meetings dropdown for forms.</summary>
//        Task<List<MeetingSelectDto>> GetMeetingSelectListAsync();

//        /// <summary>SqlKata — returns calendar events dropdown for forms.</summary>
//        Task<List<CalendarEventSelectDto>> GetCalendarEventSelectListAsync();

//        /// <summary>
//        /// EF Core — looks up an existing Tag by name so the command handlers
//        /// can reuse it instead of inserting a duplicate.
//        /// Returns null if the tag doesn't exist yet.
//        /// </summary>
//        Task<Tag?> GetTagByNameAsync(string name);
//    }
//}
