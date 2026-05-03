using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notes.Commands.DeleteNoteCommand
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly INoteRepository _noteRepository;

        public DeleteNoteCommandHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetAsync(x => x.Id == request.Id);
            if (note == null)
                throw new NotFoundException("Note not found");

            if (note.AuthorId != request.RequesterId)
                throw new NotFoundException("You are not allowed to delete this note");
            //throw new ForbiddenException("You are not allowed to delete this note");

            _noteRepository.Delete(note);
            await _noteRepository.SaveChangesAsync();

            return true;
        }
    }
}