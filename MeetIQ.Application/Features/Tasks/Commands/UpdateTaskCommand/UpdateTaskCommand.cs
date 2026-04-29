using MediatR;
using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    public class UpdateTaskCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? MeetingId { get; set; }


    }
}
