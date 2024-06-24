using Domain.Abstractions;
using Domain.Aggregates;
using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class ItemType(ItemTypeId id) : Entity<ItemTypeId>(id)
{
    public string Name { get; init; } = default!;

    public string Description { get; init; } = default!;

    public float QualityMin { get; init; }

    public float QualityMax { get; init; }

    public bool StatTrackAvailable { get; init; }

    public ItemRarity Rarity { get; init; }

    public ItemImage Image { get; init; } = default!;

    public Item NewItem(User owner, DateTime created, string createdBy) => new(ItemId.NewItemId())
    {
        Type = this,
        Quality = ItemQuality.NewItemQuality(QualityMin, QualityMax, StatTrackAvailable),
        Owner = owner,
        Created = created,
        CreatedBy = createdBy,
    };

    public static ItemType RandomItemType(string name, string? description = default)
    {
        var image = new ItemImage(
            "https://via.placeholder.com/256?text=factory_new",
            "https://via.placeholder.com/256?text=minimal_wear",
            "https://via.placeholder.com/256?text=field_tested",
            "https://via.placeholder.com/256?text=well_worn",
            "https://via.placeholder.com/256?text=battle_scarred"
        );

        var rarity = Random.Shared.GetItems(Enum.GetValues<ItemRarity>(), 1)[0];

        return new ItemType(ItemTypeId.NewItemTypeId())
        {
            Name = name,
            Description = description ?? "not provided",
            QualityMin = Random.Shared.NextSingleBetween(0f, 0.1f),
            QualityMax = Random.Shared.NextSingleBetween(0.4f, 1f),
            StatTrackAvailable = Random.Shared.Next(0, 10) == 1,
            Rarity = rarity,
            Image = image,
            Created = DateTime.UtcNow,
            CreatedBy = "system",
        };
    }
}