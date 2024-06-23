using Domain.Abstractions;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record ItemQuality(float Value, bool IsStatTrack) : IValueObject
{
    public ItemQualityType QualityType => Value switch
    {
        < 0.15f => ItemQualityType.FactoryNew,
        < 0.38f => ItemQualityType.MinimalWear,
        < 0.45f => ItemQualityType.FieldTested,
        < 0.55f => ItemQualityType.WellWorn,
        _ => ItemQualityType.BattleScarred,
    };

    public string DisplayName => IsStatTrack
        ? $"StatTrack {QualityType.ToDisplayString()}"
        : QualityType.ToDisplayString();

    public static ItemQuality NewItemQuality(float minRarity, float maxRarity, bool isStatTrack) =>
        new((float) Random.Shared.NextDouble() * (maxRarity - minRarity) + minRarity, isStatTrack);
}