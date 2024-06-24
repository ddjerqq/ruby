using Domain.Abstractions;
using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class Item(ItemId id) : Entity<ItemId>(id)
{
    public ItemType Type { get; init; } = default!;

    public ItemQuality Quality { get; init; } = default!;

    public User Owner { get; init; } = default!;
}