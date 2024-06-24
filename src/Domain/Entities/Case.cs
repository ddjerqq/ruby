using System.ComponentModel.DataAnnotations;
using Domain.Abstractions;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Entities;

[StrongId(typeof(Ulid))]
public sealed class Case(CaseId id) : Entity<CaseId>(id)
{
    public const decimal HouseEdge = 1.20m;

    private readonly List<CaseDrop> _drops = [];

    public string Name { get; init; } = default!;

    public string ImageUrl { get; init; } = default!;

    public decimal Price =>
        Drops.Sum(drop => drop.DropPrice * drop.DropChance) * HouseEdge;

    public decimal AveragePrice => Drops.Average(drop => drop.DropPrice);

    public float WinRate => (float)Drops
        .Where(drop => drop.DropPrice > AveragePrice)
        .Sum(drop => drop.DropChance);

    public float Roi => WinRate / (float)HouseEdge;

    public IEnumerable<CaseDrop> Drops => _drops;

    public CaseDrop Open()
    {
        var random = new Random();
        var dropChance = (decimal)random.NextDouble();
        var dropSum = 0.0m;

        foreach (var drop in _drops.OrderBy(x => x.DropChance))
        {
            dropSum += drop.DropChance;
            if (dropSum >= dropChance)
                return drop;
        }

        throw new InvalidOperationException("No drop found");
    }

    public static Case CreateNew(string name, string imageUrl, IEnumerable<CaseDrop> caseDrops, DateTime created, string createdBy)
    {
        caseDrops = caseDrops.ToList();

        if (caseDrops.Sum(x => x.DropChance) != 1)
            throw new InvalidOperationException("Drop chances must sum to 1");

        var @case = new Case(CaseId.NewCaseId())
        {
            Name = name,
            ImageUrl = imageUrl,
            Created = created,
            CreatedBy = createdBy,
        };

        @case._drops.AddRange(caseDrops);

        return @case;
    }
}