using LubHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Context;

/// <summary>
/// Main database context for the LubHub application
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Streamer> Streamers { get; set; }
    public DbSet<Raffle> Raffles { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Winner> Winners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}