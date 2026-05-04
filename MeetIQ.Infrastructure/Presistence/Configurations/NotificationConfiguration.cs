using MeetIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetIQ.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.ActionUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.Type)
                   .HasConversion<int>();

            builder.Property(x => x.ReferenceType)
                   .HasConversion<int?>();

            // Index for fast unread lookups per user
            builder.HasIndex(x => new { x.UserId, x.IsRead, x.IsDeleted });

            // Index for chronological feed
            builder.HasIndex(x => new { x.UserId, x.CreatedAt });

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}