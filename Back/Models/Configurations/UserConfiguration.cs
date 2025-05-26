using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Back.Models;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.bets)
            .WithOne(b => b.user);

        builder.HasMany(u => u.pictures)
            .WithOne(p => p.user);

        builder.HasMany(u => u.notifications)
            .WithOne(n => n.user);
    }
}