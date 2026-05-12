using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notes.Commands.DeleteNoteCommand
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand,bool>
    {
        private readonly INoteRepository repo;

        public DeleteNoteCommandHandler(INoteRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await repo.GetAsync(x => x.Id == request.Id && x.AuthorId == request.RequesterId);

            if (note == null)
                throw new NotFoundException("Note not found");

            // already deleted
            if (note.IsDeleted)
                throw new BadRequestException("Note already deleted");

            // Soft delete
            note.IsDeleted = true;

            repo.Update(note);
            await repo.SaveChangesAsync();

            return true;
        }
    }

}