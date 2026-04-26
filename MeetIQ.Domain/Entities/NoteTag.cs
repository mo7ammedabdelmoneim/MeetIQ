namespace MeetIQ.Domain.Entities
{
    public class NoteTag
    {
        public Guid NoteId { get; set; }
        public Note Note { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }

   
}
