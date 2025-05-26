using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Back.Models;

public class PictureConfiguration : IEntityTypeConfiguration<PictureEntity>
{
    public void Configure(EntityTypeBuilder<PictureEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.bets)
            .WithOne(b => b.picture);

        builder.HasOne(p => p.user)
            .WithMany(u => u.pictures)
            .HasForeignKey(p => p.UserId);
    }
}