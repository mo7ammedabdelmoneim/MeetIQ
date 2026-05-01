using MediatR;
using MeetIQ.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand
{
    public class JoinMeetingCommand : ICommand<Unit>
    {
        public Guid MeetingId { get; set; }
    }
}
