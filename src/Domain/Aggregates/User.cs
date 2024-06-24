using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Aggregates;

[StrongId(typeof(Ulid))]
public sealed class User(UserId id) : AggregateRoot<UserId>(id)
{
    public string Username { get; init; } = default!;

    public Level Level { get; init; } = default!;

    public Wallet Wallet { get; init; } = default!;

    public ICollection<Case> CaseInventory { get; init; } = [];

    public ICollection<Item> ItemInventory { get; init; } = [];
}