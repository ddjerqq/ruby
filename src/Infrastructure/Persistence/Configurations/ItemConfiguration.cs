using System.ComponentModel;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ruby.Generated;

namespace Infrastructure.Persistence.Configurations;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(64);

        builder.ComplexProperty(x => x.Quality, qualityBuilder =>
        {
            qualityBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
            qualityBuilder.Property(x => x.Value).HasColumnName("quality");
            qualityBuilder.Property(x => x.IsStatTrack).HasColumnName("is_stat_track");
        });

        builder.ComplexProperty(x => x.Image, imageBuilder =>
        {
            imageBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

            imageBuilder.Property(x => x.FactoryNew).HasColumnName("factory_new_img");
            imageBuilder.Property(x => x.MinimalWear).HasColumnName("minimal_wear_img");
            imageBuilder.Property(x => x.FieldTested).HasColumnName("field_tested_img");
            imageBuilder.Property(x => x.WellWorn).HasColumnName("well_worn_img");
            imageBuilder.Property(x => x.BattleScarred).HasColumnName("battle_scarred_img");
        });

        builder.HasOne(item => item.Owner)
            .WithMany(owner => owner.Inventory)
            .HasForeignKey(item => item.OwnerId);

        // one day, when EF core supports seeding Complex properties
        // builder.HasData(SeedData());
    }

    private static IEnumerable<Item> SeedData()
    {
        // dt: 2024-01-01
        // ts: 1704067200
        // id: 0001JS40400000000000000000

        yield return new Item(ItemId.Parse("item_0001js40400000000000000000"))
        {
            Name = "Sawed-Off - Orange DDPAT",
            Quality = ItemQuality.NewItemQuality(0, 1, false),
            Image = new ItemImage(
                "https://qu.ax/tQpS.png",
                "https://qu.ax/tQpS.png",
                "https://qu.ax/MsOC.png",
                "https://qu.ax/MsOC.png",
                "https://qu.ax/Xwer.png"
            ),
            Rarity = ItemRarity.ConsumerGrade,
            OwnerId = UserId.Parse("user_0001js40400000000000000000"),
            Created = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system",
        };

        yield return new Item(ItemId.Parse("item_0001js40400000000000000001"))
        {
            Name = "AWP - PAW",
            Quality = ItemQuality.NewItemQuality(0, 1, true),
            Image = new ItemImage(
                "https://qu.ax/BazS.png",
                "https://qu.ax/BazS.png",
                "https://qu.ax/agVW.png",
                "https://qu.ax/agVW.png",
                "https://qu.ax/gwi.png"
            ),
            Rarity = ItemRarity.Restricted,
            OwnerId = UserId.Parse("user_0001js40400000000000000000"),
            Created = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system",
        };
    }
}