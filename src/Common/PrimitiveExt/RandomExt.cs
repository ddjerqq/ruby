namespace Ruby.Common.PrimitiveExt;

public static class RandomExt
{
    public static float NextSingleBetween(this Random random, float min, float max) => random.NextSingle() * (max - min) + min;
}