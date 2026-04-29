using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.DTOs
{
    public class MeetingSelectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
