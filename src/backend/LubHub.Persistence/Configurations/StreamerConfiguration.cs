using LubHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LubHub.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Streamer entity
/// </summary>
public class StreamerConfiguration : IEntityTypeConfiguration<Streamer>
{
    /// <summary>
    /// Configures the Streamer entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<Streamer> builder)
    {
        builder.ToTable("streamers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TwitchId).IsRequired();
        builder.HasIndex(x => x.TwitchId).IsUnique();

        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Email).IsRequired().HasMaxLength(200);

        builder.HasMany(x => x.Raffles).WithOne().HasForeignKey(x => x.StreamerId).OnDelete(DeleteBehavior.Cascade);
    }
}