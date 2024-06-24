using Domain.Aggregates;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Application.Dtos;

public record CaseDto
{
    public string Name { get; init; } = default!;

    public string ImageUrl { get; init; } = default!;

    public IEnumerable<CaseDrop> Drops { get; init; } = default!;

    public UserId OwnerId { get; init; } = default!;

    public decimal Price { get; init; }

    public decimal AveragePrice { get; init; }

    public float WinRate { get; init; }

    public float Roi { get; init; }
}