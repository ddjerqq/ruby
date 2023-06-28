namespace Ruby.Api.Auth;

/// <summary>
/// Snowflakes
/// Discord utilizes Twitter's snowflake format for uniquely identifiable descriptors (IDs).
///     These IDs are guaranteed to be unique across all of Discord, except in some unique scenarios in which child objects share their parent's ID.
///     Because Snowflake IDs are up to 64 bits in size (e.g. an uint64), they are always returned as strings in the HTTP API to prevent integer overflows in some languages.
///     See Gateway ETF/JSON for more information regarding Gateway encoding.
///     Snowflake ID Broken Down in Binary:
/// 111111111111111111111111111111111111111111 11111 11111 111111111111
/// 64                                         22    17    12          0
/// Snowflake ID Format Structure (Left to Right):
/// FIELD | BITS | NUMBER OF BITS | DESCRIPTION | RETRIEVAL
/// Timestamp | 63 to 22 | 42 bits | Milliseconds since Discord Epoch, the first second of 2015 or 1420070400000. | (snowflake >> 22) + 1420070400000
/// Internal worker ID | 21 to 17 | 5 bits | | (snowflake & 0x3E0000) >> 17
/// Internal process ID | 16 to 12 | 5 bits | | (snowflake & 0x1F000) >> 12
/// Increment | 11 to 0 | 12 bits | For every ID that is generated on that process, this number is incremented | snowflake & 0xFFF
/// source: https://discord.com/developers/docs/reference#snowflakes
/// </summary>
public static class Snowflake
{
    /// <summary>
    /// January 1st 2015, the first second of 2015 * 100
    /// </summary>
    public const ulong SnowflakeEpoch = 1420070400000;

    /// <summary>
    /// create a new snowflake
    /// </summary>
    /// <returns></returns>
    public static ulong NewSnowflake()
    {
        var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var id = (ulong) ts.TotalMilliseconds;

        id -= SnowflakeEpoch;
        id <<= 5;

        // internal worker id simulation
        id += (ulong) Random.Shared.Next(0, 32); // 5 bits
        id <<= 5;

        // internal process id simulation
        id += (ulong) Random.Shared.Next(0, 32); // 5 bits
        id <<= 12;

        // for every ID that is generated on a process, this number is incremented
        id += (ulong) Random.Shared.Next(0, 4096); // 12 bits
        return id;
    }

    /// <summary>
    /// get the UTC DateTime when the snowflake was created
    /// </summary>
    /// <param name="snowflake"></param>
    /// <returns></returns>
    public static DateTime CreatedAt(this ulong snowflake)
    {
        var ts = ((snowflake >> 22) + SnowflakeEpoch) / 100;
        return DateTimeOffset.FromUnixTimeMilliseconds((long) ts).UtcDateTime;
    }
};