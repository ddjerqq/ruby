using Domain.Abstractions;
using Domain.Entities;
using Ruby.Generated;

namespace Domain.ValueObjects;

public sealed record CaseDrop : IValueObject
{
    public ItemTypeId ItemTypeId { get; init; }
    public ItemType ItemType { get; init; } = default!;

    public CaseTypeId CaseTypeId { get; init; }
    public CaseType CaseType { get; init; } = default!;

    public decimal DropChance { get; init; }

    public decimal DropPrice { get; init; }
}