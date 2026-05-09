using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Persistence.Configurations;
using MeetIQ.Infrastructure.Presistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetIQ.Infrastructure.Presistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CalendarEvent>  CalendarEvents{ get; set; }
        public DbSet<FeedbackReport> FeedbackReports { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public DbSet<MeetingInvitation> MeetingInvitations { get; set; }
        public DbSet<MeetingSummary> MeetingSummaries { get; set; }
        public DbSet<MeetingTranscript> MeetingTranscripts { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskItem>  TaskItems { get; set; }
        public DbSet<Notification>  Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NoteTag>()
             .HasKey(x => new { x.NoteId, x.TagId });


            builder.ApplyConfiguration(new CalendarEventConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new MeetingInvitationConfiguration());
        }
    }
}
