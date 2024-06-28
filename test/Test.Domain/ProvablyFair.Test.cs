using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace Test.Domain;

public static class ProvablyFair
{
    /// <summary>
    /// every 33rd game will crash, so the house edge is 3.(3)%
    /// if the value was 25, the house edge would be 4%
    /// 100 / 33 = 3.(03)
    /// </summary>
    public const int HouseEdge = 33;

    public const ushort MaxGames = ushort.MaxValue;

    // secret value. This is the server seed, the hash of the first game.
    private const string ServerSeed = "a03f13e5120543c7af78bca96408c7817afed38b71204b75adfd00188a202965";
    private const string Salt = "d2867566759e9158bda9bf93b343bbd9aa02ce1e0c5bc2b37a2d70d391b04f14";
    private static readonly ulong E = (ulong)BigInteger.Pow(2, 52);

    public static readonly string[] Games;

    static ProvablyFair()
    {
        var gameHash = ServerSeed;

        Games = new string[MaxGames];
        for (var i = 0; i < MaxGames; i++)
        {
            gameHash = HashHex(gameHash);
            Games[i] = gameHash;
        }
    }

    public static double CrashPointFromGameHash(string input)
    {
        var hash = HmacHash(input);
        var hashValue = BigInteger.Parse(hash, NumberStyles.HexNumber);

        if (hashValue % HouseEdge == 0)
            return 1;

        var h = ulong.Parse(hash[..13], NumberStyles.HexNumber);

        var multiplier = (100 * E - h) / (E - h);
        return Math.Floor((double)multiplier) / 100.0;
    }

    public static string GetPreviousGameHash(string gameHash) => HashHex(gameHash);

    // need to index, find, and then get the previous one, the hash that this one is made from, bc if you hash the previous one, you will get this
    // ie, the next game for a game, is a hash, that when hashed, will give you the current game.
    // so it is impossible to get the next game.
    public static string GetNextGameHash(string gameHash)
    {
        // find the index of the game hash
        var index = Array.IndexOf(Games, gameHash);

        if (index == -1)
            throw new ArgumentException("Game hash not found", nameof(gameHash));

        // get the previous game hash
        return Games[index - 1];
    }

    public static string GetGameHash(ushort index) => index < MaxGames
        ? Games[MaxGames - 1 - index]
        : throw new IndexOutOfRangeException($"Index out of range, maximum number of games is {MaxGames}");

    private static string HashHex(string input) => Convert.ToHexString(SHA256.HashData(Convert.FromHexString(input)));

    private static string HmacHash(string input) => Convert.ToHexString(HMACSHA256.HashData(Convert.FromHexString(Salt), Convert.FromHexString(input)));
}

public sealed class ProvablyFairTest
{
    [Test]
    public void TestProvablyFair()
    {
        string previousHash = null;
        List<double> crashPoints = new List<double>(ProvablyFair.MaxGames);

        for (var i = 0; i < ProvablyFair.MaxGames; i++)
        {
            var gameHash = ProvablyFair.GetGameHash((ushort)i);
            var crashPoint = ProvablyFair.CrashPointFromGameHash(gameHash);
            crashPoints.Add(crashPoint);

            Console.WriteLine($"{crashPoint} {gameHash}");
            Console.WriteLine();

            if (previousHash is not null)
                Assert.That(previousHash, Is.EqualTo(ProvablyFair.GetPreviousGameHash(gameHash)));

            previousHash = gameHash;
        }

        var distribution = crashPoints.GroupBy(c => Math.Round(c, 1))
            .Select(g => (g.Key, g.Count()))
            .ToDictionary();

        Console.WriteLine(distribution);
    }
}