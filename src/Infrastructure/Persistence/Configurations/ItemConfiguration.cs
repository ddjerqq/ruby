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

        builder.Ignore(x => x.ImageUrl);

        builder.HasOne(x => x.ItemType)
            .WithMany()
            .HasForeignKey(item => item.ItemTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // an item does not exist without an item type
        builder.Navigation(x => x.ItemType)
            .AutoInclude();

        builder.ComplexProperty(x => x.Quality, qualityBuilder =>
        {
            qualityBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
            qualityBuilder.Property(x => x.Value).HasColumnName("quality");
            qualityBuilder.Property(x => x.IsStatTrack).HasColumnName("is_stat_track");
        });

        builder.HasOne(item => item.Owner)
            .WithMany(owner => owner.ItemInventory)
            .HasForeignKey(item => item.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}