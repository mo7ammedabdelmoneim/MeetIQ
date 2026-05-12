using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetIQ.Domain.Entities;
namespace MeetIQ.Infrastructure.Presistence.Configurations
{
    public class MeetingInvitationConfiguration : IEntityTypeConfiguration<MeetingInvitation>
    {
        public void Configure(EntityTypeBuilder<MeetingInvitation> builder)
        {
            builder.HasOne(e => e.InvitedUser)
                    .WithMany()
                    .HasForeignKey(e => e.InvitedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.InvitedBy)
                    .WithMany()
                    .HasForeignKey(e => e.InvitedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Meeting)
                    .WithMany(m => m.Invitations)
                    .HasForeignKey(e => e.MeetingId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}