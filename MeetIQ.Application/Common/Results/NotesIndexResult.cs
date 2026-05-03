using MeetIQ.Application.Features.Notes.DTOs;

namespace MeetIQ.Application.Common.Results
{
    public class NotesIndexResult
    {
        public IEnumerable<NoteCardDto> Notes { get; set; } = [];
        public int Total { get; set; }
        public IEnumerable<string> AllTags { get; set; } = [];
    }
}
