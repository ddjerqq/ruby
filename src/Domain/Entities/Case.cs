using Domain.Abstractions;
using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class Case(CaseId id) : Entity<CaseId>(id)
{
    public CaseTypeId CaseTypeId { get; init; }
    public CaseType CaseType { get; init; } = default!;

    public bool IsOpened { get; private set; }

    public DateTime? OpenedOn { get; private set; }

    public UserId OwnerId { get; init; }
    public User Owner { get; init; } = default!;

    public CaseDrop Open(DateTime openedOn, string openedBy)
    {
        if (IsOpened)
            throw new InvalidOperationException("Case is already opened");

        var random = new Random();
        var dropChance = (decimal)random.NextDouble();
        var dropSum = 0.0m;

        IsOpened = true;
        OpenedOn = openedOn;

        LastModified = openedOn;
        LastModifiedBy = openedBy;

        foreach (var drop in CaseType.Drops.OrderBy(x => x.DropChance))
        {
            dropSum += drop.DropChance;
            if (dropSum >= dropChance)
                return drop;
        }

        throw new InvalidOperationException("No drop found");
    }
}