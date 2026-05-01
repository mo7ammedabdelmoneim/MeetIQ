using MeetIQ.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommand : ICommand<Guid>
    {
        public string Title { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
