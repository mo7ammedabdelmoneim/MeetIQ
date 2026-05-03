using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Notes.Commands.DeleteNoteCommand
{
    public class DeleteNoteCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public string RequesterId { get; set; }
    }
}