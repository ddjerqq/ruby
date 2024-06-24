using Domain.Abstractions;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class CaseType(CaseTypeId id) : Entity<CaseTypeId>(id)
{
    public const decimal HouseEdge = 1.20m;

    public string Name { get; init; } = default!;

    public string ImageUrl { get; init; } = default!;

    public IEnumerable<CaseDrop> Drops { get; init; } = [];

    public decimal Price => Drops.Sum(drop => drop.DropPrice * drop.DropChance) * HouseEdge;

    public decimal AveragePrice => Drops.Average(drop => drop.DropPrice);

    public float WinRate => (float)Drops
        .Where(drop => drop.DropPrice > AveragePrice)
        .Sum(drop => drop.DropChance);

    public float Roi => WinRate / (float)HouseEdge;

    public static CaseType CreateNew(string name, string imageUrl, IEnumerable<CaseDrop> caseDrops, DateTime created, string createdBy)
    {
        caseDrops = caseDrops.ToList();

        if (caseDrops.Sum(x => x.DropChance) != 1)
            throw new InvalidOperationException("Drop chances must sum to 1");

        var @case = new CaseType(CaseTypeId.NewCaseTypeId())
        {
            Name = name,
            ImageUrl = imageUrl,
            Created = created,
            CreatedBy = createdBy,
            Drops = caseDrops,
        };

        return @case;
    }
}