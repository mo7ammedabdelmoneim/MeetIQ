using Microsoft.AspNetCore.Http;

namespace MeetIQ.Application.Interfaces.Services
{
    public interface IRecordingService
    {
        /// Saves the uploaded audio file to disk and persists the path in the DB.
        /// Returns the saved file path relative to wwwroot.
        Task<string> SaveAsync(Guid meetingId, IFormFile audio, CancellationToken ct = default);

        /// Returns the absolute file-system path of the recording for a meeting,
        /// or null if no recording exists yet.
        Task<string?> GetPathAsync(Guid meetingId, CancellationToken ct = default);
    }
}