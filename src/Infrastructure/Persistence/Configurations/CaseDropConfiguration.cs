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

        builder.HasKey(drop => new { drop.CaseId, drop.ItemTypeId });

        builder.HasOne(drop => drop.Case)
            .WithMany(@case => @case.Drops)
            .HasForeignKey(drop => drop.CaseId);
    }
}