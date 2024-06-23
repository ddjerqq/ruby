using Domain.Abstractions;

namespace Domain.ValueObjects;

public record Level(int Value) : IValueObject
{
    public string DisplayName => (Value / 100) switch
    {
        0 => "Plastic",
        1 => "Copper",
        2 => "Bronze",
        3 => "Silver",
        4 => "Gold",
        5 => "Platinum",
        _ => "Ruby",
    };
}