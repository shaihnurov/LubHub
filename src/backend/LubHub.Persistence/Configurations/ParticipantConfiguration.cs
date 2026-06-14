using LubHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LubHub.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Participant entity
/// </summary>
public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    /// <summary>
    /// Configures the Participant entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TwitchUserId).IsRequired();

        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(100);

        builder.Property(x => x.BotScore).IsRequired();

        builder.HasIndex(x => new { x.RaffleId, x.TwitchUserId }).IsUnique();
    }
}