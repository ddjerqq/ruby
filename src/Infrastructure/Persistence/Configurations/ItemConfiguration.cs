using System.ComponentModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasOne(x => x.Type)
            .WithMany();

        builder.ComplexProperty(x => x.Quality, qualityBuilder =>
        {
            qualityBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
            qualityBuilder.Property(x => x.Value).HasColumnName("quality");
            qualityBuilder.Property(x => x.IsStatTrack).HasColumnName("is_stat_track");
        });

        builder.HasOne(item => item.Owner)
            .WithMany(owner => owner.Inventory);
    }
}