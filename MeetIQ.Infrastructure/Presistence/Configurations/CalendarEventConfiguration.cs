using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Infrastructure.Presistence.Configurations
{
    public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
    {
        public void Configure(EntityTypeBuilder<CalendarEvent> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasMaxLength(2000);

            builder.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(7)
                .HasDefaultValue("#3B82F6");

            builder.Property(e => e.OwnerId)
                .IsRequired();

            // Soft-delete shadow property (matches your IsDeleted convention)
            builder.Property<bool>("IsDeleted")
                .HasDefaultValue(false);

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));

            // Relationships
            builder.HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Meeting)
                .WithMany()
                .HasForeignKey(e => e.MeetingId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Notes)
                .WithOne()
                .HasForeignKey(n => n.CalendarEventId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("CalendarEvents");
        }
    }
}