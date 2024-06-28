namespace Domain.Enums;

public enum ItemQualityType
{
    FactoryNew = 0,
    MinimalWear = 1,
    FieldTested = 2,
    WellWorn = 3,
    BattleScarred = 4,
}

public static class ItemQualityExt
{
    public static string ToDisplayString(this ItemQualityType qualityType) => qualityType switch
    {
        ItemQualityType.FactoryNew => "Factory New",
        ItemQualityType.MinimalWear => "Minimal Wear",
        ItemQualityType.FieldTested => "Field Tested",
        ItemQualityType.WellWorn => "Well Worn",
        ItemQualityType.BattleScarred => "Battle Scarred",
        _ => throw new ArgumentOutOfRangeException(nameof(qualityType), qualityType, null)
    };
}