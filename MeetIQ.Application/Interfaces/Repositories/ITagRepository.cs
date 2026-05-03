using MeetIQ.Application.Features.Notes.DTOs;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<TagDto>> GetAllAsync();
        Task<Tag> GetOrCreateAsync(string name);
        Task<IEnumerable<string>> GetAllNamesAsync();
    }
}