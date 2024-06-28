using Domain.Abstractions;
using Domain.Enums;
using Ruby.Common.PrimitiveExt;

namespace Domain.ValueObjects;

public sealed record ItemQuality(float Value, bool IsStatTrack) : IValueObject
{
    public ItemQualityType QualityType => Value switch
    {
        < 0.07f => ItemQualityType.FactoryNew,
        < 0.15f => ItemQualityType.MinimalWear,
        < 0.37f => ItemQualityType.FieldTested,
        < 0.45f => ItemQualityType.WellWorn,
        _ => ItemQualityType.BattleScarred,
    };

    public string DisplayName => IsStatTrack
        ? $"StatTrack {QualityType.ToDisplayString()}"
        : QualityType.ToDisplayString();

    public static ItemQuality NewItemQuality(float minRarity, float maxRarity, bool isStatTrack) =>
        new(Random.Shared.NextSingleBetween(minRarity, maxRarity), isStatTrack);
}