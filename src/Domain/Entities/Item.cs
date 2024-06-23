using Domain.Abstractions;
using Domain.Aggregates;
using Domain.Enums;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class Item(ItemId id) : Entity<ItemId>(id)
{
    public string Name { get; init; } = default!;

    public ItemQuality Quality { get; init; } = default!;

    public ItemImage Image { get; init; } = default!;

    public ItemRarity Rarity { get; init; }

    public UserId OwnerId { get; init; }

    public User Owner { get; init; } = default!;

    public static Item RandomItem(string? name = default)
    {
        var quality = ItemQuality.NewItemQuality(0.0f, 1.0f, Random.Shared.Next(0, 100) < 10);

        var image = new ItemImage(
            "https://via.placeholder.com/256?text=factory_new",
            "https://via.placeholder.com/256?text=minimal_wear",
            "https://via.placeholder.com/256?text=field_tested",
            "https://via.placeholder.com/256?text=well_worn",
            "https://via.placeholder.com/256?text=battle_scarred"
        );

        var rarity = Random.Shared.Next(0, 7) switch
        {
            // ConsumerGrade
            0 => ItemRarity.ConsumerGrade,
            // IndustrialGrade
            1 => ItemRarity.IndustrialGrade,
            // MilSpec
            2 => ItemRarity.MilSpec,
            // Restricted
            3 => ItemRarity.Restricted,
            // Classified
            4 => ItemRarity.Classified,
            // Covert
            5 => ItemRarity.Covert,
            // Extraordinary
            6 => ItemRarity.Extraordinary,
            // Contraband
            _ => ItemRarity.Contraband,
        };

        return new Item(ItemId.NewItemId())
        {
            Name = name ?? "Random Item",
            Quality = quality,
            Image = image,
            Rarity = rarity,
        };
    }
}