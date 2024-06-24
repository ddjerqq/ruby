using System.ComponentModel;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseDropConfiguration : IEntityTypeConfiguration<CaseDrop>
{
    public void Configure(EntityTypeBuilder<CaseDrop> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasKey(drop => new { CaseId = drop.CaseTypeId, drop.ItemTypeId });

        builder.HasOne(drop => drop.CaseType)
            .WithMany(@case => @case.Drops)
            .HasForeignKey(drop => drop.CaseTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(drop => drop.ItemType)
            .WithMany()
            .HasForeignKey(drop => drop.ItemTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}