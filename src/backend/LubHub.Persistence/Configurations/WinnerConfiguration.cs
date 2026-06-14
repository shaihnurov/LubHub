using LubHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LubHub.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Winner entity
/// </summary>
public class WinnerConfiguration : IEntityTypeConfiguration<Winner>
{
    /// <summary>
    /// Configures the Winner entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<Winner> builder)
    {
        builder.ToTable("winners");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TwitchUserId).IsRequired();

        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(100);

        builder.HasOne<Participant>().WithOne().HasForeignKey<Winner>(x => x.ParticipantId).OnDelete(DeleteBehavior.Restrict);
    }
}