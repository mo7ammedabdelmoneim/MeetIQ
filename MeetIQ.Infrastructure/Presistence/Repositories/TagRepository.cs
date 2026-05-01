using MeetIQ.Application.Common.Options;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }

    }
}
