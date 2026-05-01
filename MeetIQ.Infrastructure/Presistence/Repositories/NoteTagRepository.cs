using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class NoteTagRepository : Repository<NoteTag>, INoteTagRepository
    {
        public NoteTagRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }
    }
}
