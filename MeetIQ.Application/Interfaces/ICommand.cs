using MediatR;

namespace MeetIQ.Application.Interfaces
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
