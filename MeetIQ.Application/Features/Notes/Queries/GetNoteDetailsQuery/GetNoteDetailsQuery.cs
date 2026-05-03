using MediatR;
using MeetIQ.Application.Features.Notes.DTOs;

namespace MeetIQ.Application.Features.Notes.Queries.GetNoteDetailsQuery
{
    public class GetNoteDetailsQuery : IRequest<NoteDetailsDto>
    {
        public Guid Id { get; set; }
        public string RequesterId { get; set; } = string.Empty;
    }
}