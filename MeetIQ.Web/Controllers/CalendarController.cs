using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MeetIQ.Application.Features.Calendar.Commands.CreateCalendarEventCommand;
using MeetIQ.Application.Features.Calendar.Commands.DeleteCalendarEventCommand;
using MeetIQ.Application.Features.Calendar.Commands.UpdateCalendarEventCommand;
using MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventByIdQuery;
using MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using MeetIQ.Web.ViewModels.Calendar;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
            => _mediator = mediator;

        // Helpers 
        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private async Task<List<SelectListItem>> GetMeetingSelectListAsync()
        {
            var meetings = await _mediator.Send(
                new GetUserMeetingsQuery { UserId = UserId });

            return meetings
                .Select(m => new SelectListItem(m.Title, m.Id.ToString()))
                .ToList();
        }

        // GET /Calendar 
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Calendar";
            return View();
        }

        // (AJAX – FullCalendar feed) 
        [HttpGet]
        public async Task<IActionResult> Events(DateTime start, DateTime end)
        {
            var events = await _mediator.Send(new GetCalendarEventsQuery
            {
                UserId = UserId,
                RangeFrom = start,
                RangeTo = end
            });

            // Shape matches FullCalendar's EventInput contract
            var payload = events.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartTime.ToString("o"),   
                end = e.EndTime.ToString("o"),
                color = e.Color,
                extendedProps = new { e.Description, e.MeetingId, e.MeetingTitle }
            });

            return Json(payload);
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var ev = await _mediator.Send(new GetCalendarEventByIdQuery
            {
                Id = id,
                UserId = UserId
            });

            if (ev is null) return NotFound();

            ViewData["Title"] = ev.Title;
            return View(ev);
        }

        // GET Create 
        [HttpGet]
        public async Task<IActionResult> Create(DateTime? start)
        {
            ViewData["Title"] = "New Event";

            var vm = new CreateCalendarEventViewModel
            {
                StartTime = start ?? DateTime.Today.AddHours(9),
                EndTime = (start ?? DateTime.Today.AddHours(9)).AddHours(1),
                Meetings = await GetMeetingSelectListAsync()
            };

            return View(vm);
        }

        // POST Create 
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCalendarEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Meetings = await GetMeetingSelectListAsync();
                return View(model);
            }

            var id = await _mediator.Send(new CreateCalendarEventCommand
            {
                Title = model.Title,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Color = model.Color,
                MeetingId = model.MeetingId,
                OwnerId = UserId
            });

            TempData["Success"] = "Event created successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var ev = await _mediator.Send(new GetCalendarEventByIdQuery
            {
                Id = id,
                UserId = UserId
            });

            if (ev is null) return NotFound();

            ViewData["Title"] = "Edit Event";

            var vm = new EditCalendarEventViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                Color = ev.Color,
                MeetingId = ev.MeetingId,
                Meetings = await GetMeetingSelectListAsync()
            };

            return View(vm);
        }

        // POST Edit
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCalendarEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Meetings = await GetMeetingSelectListAsync();
                return View(model);
            }

            await _mediator.Send(new UpdateCalendarEventCommand
            {
                Id = id,
                OwnerId = UserId,
                Title = model.Title,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Color = model.Color,
                MeetingId = model.MeetingId
            });

            TempData["Success"] = "Event updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Delete
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCalendarEventCommand
            {
                Id = id,
                OwnerId = UserId
            });

            TempData["Success"] = "Event deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}