using MediatR;
using MeetIQ.Application.Features.Tasks.Queries.GetPendingTasksCountQuery;
using Microsoft.AspNetCore.Mvc;

namespace MeetIQ.Web.ViewComponents
{
    public class TasksCountViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public TasksCountViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = await _mediator.Send(new GetPendingTasksCountQuery());

            return View(count);
        }
    }
}
