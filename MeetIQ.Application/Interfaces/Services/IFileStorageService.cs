using Microsoft.AspNetCore.Http;

namespace MeetIQ.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(string filePath);
    }
}