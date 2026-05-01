using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Queries.GetPendingTasksCountQuery
{
    public class GetPendingTasksCountQuery : IRequest<int>
    {
        public string UserId { get; set; }
    }
}
