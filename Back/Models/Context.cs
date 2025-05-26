using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Back.Models;

public class Context : DbContext
{
    public DbSet<BetEntity> Bets { get; set; } = null!;
    public DbSet<NotificationEntity> notifications { get; set; } = null!;
    public DbSet<PictureEntity> Pictures { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    
    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BetConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new PictureConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}