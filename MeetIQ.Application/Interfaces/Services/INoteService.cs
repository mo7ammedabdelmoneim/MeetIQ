namespace MeetIQ.Application.Interfaces.Services
{
    public interface INoteService
    {
        Task<NotesIndexViewModel> GetIndexAsync(
            string userId,
            string? search,
            string? tag,
            string? filterType);

        Task<NoteDetailViewModel?> GetDetailAsync(Guid id, string userId);

        Task<NoteCreateViewModel> GetCreateFormAsync(Guid? meetingId, Guid? calendarEventId);

        Task<Guid> CreateAsync(NoteCreateViewModel vm, string userId);

        Task<NoteEditViewModel?> GetEditFormAsync(Guid id, string userId);

        Task<bool> UpdateAsync(NoteEditViewModel vm, string userId);

        Task<bool> DeleteAsync(Guid id, string userId);
    }
}
