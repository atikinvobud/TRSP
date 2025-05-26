using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Back.Models;

public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.user)
            .WithMany(u => u.notifications)
            .HasForeignKey(n => n.UserId);

        builder.HasOne(n => n.bet)
            .WithMany(b => b.notifications)
            .HasForeignKey(n => n.BetId);
    }
}