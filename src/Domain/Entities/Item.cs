using Domain.Abstractions;
using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class Item(ItemId id) : Entity<ItemId>(id)
{
    public ItemTypeId ItemTypeId { get; init; }
    public ItemType ItemType { get; init; } = default!;

    public ItemQuality Quality { get; init; } = default!;

    public string ImageUrl => ItemType.Image.GetImageForQuality(Quality.QualityType);

    public UserId OwnerId { get; init; }
    public User Owner { get; init; } = default!;
}