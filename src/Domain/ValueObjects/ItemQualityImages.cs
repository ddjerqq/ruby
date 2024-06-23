using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record ItemImage(
    string FactoryNew,
    string MinimalWear,
    string FieldTested,
    string WellWorn,
    string BattleScarred)
{
    public string GetImageForQuality(ItemQualityType qualityType) => qualityType switch
    {
        ItemQualityType.FactoryNew => FactoryNew,
        ItemQualityType.MinimalWear => MinimalWear,
        ItemQualityType.FieldTested => FieldTested,
        ItemQualityType.WellWorn => WellWorn,
        ItemQualityType.BattleScarred => BattleScarred,
        _ => throw new ArgumentOutOfRangeException(nameof(qualityType), qualityType, null)
    };
}