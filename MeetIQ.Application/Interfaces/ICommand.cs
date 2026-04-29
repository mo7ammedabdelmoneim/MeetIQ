using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Interfaces
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
