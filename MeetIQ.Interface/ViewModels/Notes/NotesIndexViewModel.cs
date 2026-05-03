namespace MeetIQ.Interface.ViewModels.Notes
{

    public class NotesIndexViewModel
    {
        public IEnumerable<NoteCardViewModel> Notes { get; set; } = [];
        public string? SearchQuery { get; set; }
        public string? SelectedTag { get; set; }

        public string? FilterType { get; set; }

        public IEnumerable<string> AllTags { get; set; } = [];
        public int TotalCount { get; set; }

        public int CurrentPage { get; set; } = 1;

    }
}