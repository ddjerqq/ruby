using System.ComponentModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
{
    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.Property(x => x.Name)
            .HasMaxLength(64);

        builder.Property(x => x.Description)
            .HasMaxLength(256);

        builder.ComplexProperty(x => x.Image, imageBuilder =>
        {
            imageBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

            imageBuilder.Property(x => x.FactoryNew).HasColumnName("factory_new_img");
            imageBuilder.Property(x => x.MinimalWear).HasColumnName("minimal_wear_img");
            imageBuilder.Property(x => x.FieldTested).HasColumnName("field_tested_img");
            imageBuilder.Property(x => x.WellWorn).HasColumnName("well_worn_img");
            imageBuilder.Property(x => x.BattleScarred).HasColumnName("battle_scarred_img");
        });

        builder.HasMany<Item>()
            .WithOne(item => item.Type);
    }
}