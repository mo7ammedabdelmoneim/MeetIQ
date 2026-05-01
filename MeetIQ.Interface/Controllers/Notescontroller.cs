using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Domain.Entities;
using MeetIQ.Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetIQ.Interface.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteService _notes;
        private readonly UserManager<ApplicationUser> _users;

        public NotesController(INoteService notes, UserManager<ApplicationUser> users)
        {
            _notes = notes;
            _users = users;
        }

        private string UserId => _users.GetUserId(User)!;

        // GET /Notes
        public async Task<IActionResult> Index(
            string? q, string? tag, string? filter)
        {
            ViewData["Title"] = "Notes";
            var vm = await _notes.GetIndexAsync(UserId, q, tag, filter);
            return View(vm);
        }

        // GET /Notes/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _notes.GetDetailAsync(id, UserId);
            if (vm is null) return NotFound();
            ViewData["Title"] = vm.Title;
            return View(vm);
        }

        // GET /Notes/Create
        public async Task<IActionResult> Create(
            Guid? meetingId, Guid? calendarEventId)
        {
            ViewData["Title"] = "New Note";
            var vm = await _notes.GetCreateFormAsync(meetingId, calendarEventId);
            return View(vm);
        }

        // POST /Notes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var fresh = await _notes.GetCreateFormAsync(vm.MeetingId, vm.CalendarEventId);
                vm.Meetings = fresh.Meetings;
                vm.CalendarEvents = fresh.CalendarEvents;
                vm.AllTags = fresh.AllTags;
                return View(vm);
            }

            var id = await _notes.CreateAsync(vm, UserId);
            TempData["Success"] = "Note created successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET /Notes/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var vm = await _notes.GetEditFormAsync(id, UserId);
            if (vm is null) return NotFound();
            ViewData["Title"] = "Edit Note";
            return View(vm);
        }

        // POST /Notes/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var fresh = await _notes.GetEditFormAsync(vm.Id, UserId);
                vm.Meetings = fresh?.Meetings ?? [];
                vm.CalendarEvents = fresh?.CalendarEvents ?? [];
                vm.AllTags = fresh?.AllTags ?? [];
                return View(vm);
            }

            var ok = await _notes.UpdateAsync(vm, UserId);
            if (!ok) return NotFound();

            TempData["Success"] = "Note updated successfully.";
            return RedirectToAction(nameof(Details), new { vm.Id });
        }

        // POST /Notes/Delete/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _notes.DeleteAsync(id, UserId);
            TempData["Success"] = "Note deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}