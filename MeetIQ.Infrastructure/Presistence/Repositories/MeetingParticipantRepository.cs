using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class MeetingParticipantRepository : Repository<MeetingParticipant>, IMeetingParticipantRepository
    {
        public MeetingParticipantRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }
    }
}
