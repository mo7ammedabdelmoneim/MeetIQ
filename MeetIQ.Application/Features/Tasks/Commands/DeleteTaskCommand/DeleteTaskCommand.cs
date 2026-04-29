using MediatR;
using MeetIQ.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Commands.DeleteTaskCommand
{
    public class DeleteTaskCommand : ICommand<Unit>
    {
        public Guid TaskId { get; set; }
    }
}
