using MeetIQ.Application.Common;
using MeetIQ.Application.Features.Notes.DTOs;
using MediatR;
using MeetIQ.Application.Common.Results;

namespace MeetIQ.Application.Features.Notes.Queries.GetNotesQuery
{
    public class GetNotesQuery : IRequest<NotesIndexResult>
    {
        public string UserId { get; set; } = string.Empty;
        public string? Search { get; set; }
        public string? Tag { get; set; }
        public string? Filter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }


}