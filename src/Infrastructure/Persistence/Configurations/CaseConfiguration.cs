using System.ComponentModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.Property(x => x.Name)
            .HasMaxLength(32);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(64);

        builder
            .Ignore(x => x.Price)
            .Ignore(x => x.AveragePrice)
            .Ignore(x => x.WinRate)
            .Ignore(x => x.Roi);

        builder
            .HasMany(@case => @case.Drops)
            .WithOne(drop => drop.Case)
            .HasForeignKey(drop => drop.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}