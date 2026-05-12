using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Notes.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notes.Queries.GetNoteDetailsQuery
{
    public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsDto>
    {
        private readonly INoteRepository _noteRepository;

        public GetNoteDetailsQueryHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<NoteDetailsDto> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetByIdAsync(request.Id);

            if (note is null)
                throw new NotFoundException("Note not found");

            if (note.AuthorId != request.RequesterId)
                throw new BadRequestException("Access denied");

            return note;
        }
    }
}