using System.ComponentModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class CaseTypeConfiguration : IEntityTypeConfiguration<CaseType>
{
    public void Configure(EntityTypeBuilder<CaseType> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.Property(x => x.Name)
            .HasMaxLength(32);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(32);

        builder.HasMany(x => x.Drops)
            .WithOne(drop => drop.CaseType)
            .HasForeignKey(drop => drop.CaseTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}