using MediatR;
using MeetIQ.Application.Features.Tasks.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Queries.GetTaskByIdQuery
{
    public class GetTaskByIdQuery : IRequest<TaskDetailsDto?>
    {
        public Guid Id { get; set; }
    }
}
