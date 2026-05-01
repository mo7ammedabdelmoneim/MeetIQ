using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class MeetingParticipantRepository : Repository<MeetingParticipant>, IMeetingParticipantRepository
    {
        public MeetingParticipantRepository(ApplicationDbContext context, QueryFactory db) : base(context, db)
        {
        }
    }
}
