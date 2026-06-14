using LubHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LubHub.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Raffle entity
/// </summary>
public class RaffleConfiguration : IEntityTypeConfiguration<Raffle>
{
    /// <summary>
    /// Configures the Raffle entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<Raffle> builder)
    {
        builder.ToTable("raffles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Status).IsRequired();

        builder.HasMany(x => x.Participants).WithOne().HasForeignKey(x => x.RaffleId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Winner).WithOne().HasForeignKey<Winner>(x => x.RaffleId).OnDelete(DeleteBehavior.Cascade);
    }
}