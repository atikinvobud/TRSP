using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Back.Models;

public class BetConfiguration : IEntityTypeConfiguration<BetEntity>
{
    public void Configure(EntityTypeBuilder<BetEntity> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.user)
            .WithMany(u => u.bets)
            .HasForeignKey(b => b.UserId);

        builder.HasOne(b => b.picture)
            .WithMany(p => p.bets)
            .HasForeignKey(b => b.PictureId);

        builder.HasMany(b => b.notifications)
            .WithOne(n => n.bet);
    }
}