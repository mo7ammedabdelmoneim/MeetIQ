using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;
using SqlKata.Execution;
using MeetIQ.Application.Features.Notes.DTOs;

namespace MeetIQ.Infrastructure.Persistence.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db) { }

        public async Task<Tag> GetOrCreateAsync(string name)
        {
            var normalized = name.Trim().ToLower();

            var existing = await context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == normalized);

            if (existing is not null) return existing;

            var tag = new Tag { Name = normalized };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
            return tag;
        }

        public async Task<IEnumerable<string>> GetAllNamesAsync()
        {
            return await db.Query("Tags")
                .Select("Name")
                .OrderBy("Name")
                .GetAsync<string>();
        }

        public async Task<List<TagDto>> GetAllAsync()
        {
            return (await db.Query("Tags")
                .Select("Id", "Name")   
                .OrderBy("Name")
                .GetAsync<TagDto>()).ToList();
        }
    }
}