using System.Text;

namespace Diamond;

// mandatory headers
// app-token
// user-id
// sign
// time

public class Rules
{
    // 32 random chars
    public string Salt { get; init; } = null!;

    // random int in range: [0x1000, 0xffff]
    // sha1 hash of the payload
    // hex of the checksum
    // random int in range: [0x10000000, 0xffffffff]
    public string HashFormat { get; init; } = null!;

    // 32 uints all in range [0, 64)
    public IEnumerable<int> ChecksumIndexes { get; init; } = null!;
    
    // random byte full range
    public byte ChecksumConstant { get; init; }

    // md5 hash of random data, probably a new guid
    public string AppToken { get; init; } = null!;

    public string SignUri(Uri uri, DateTime timestamp, long userId)
    {
        // extract uri
        var extractedUri = "";
        
        // extract timestamp milliseconds
        var timestampMs = (timestamp - new DateTime(1970, 1, 1)).TotalMilliseconds;

        // create the hash
        var payload =
            $"{Salt}" +
            $"{timestampMs}" +
            $"{extractedUri}" +
            $"{userId}";
        var digest = "";
        
        // calculate sum
        var digestBytes = Encoding.UTF8.GetBytes(digest);
        var sum = (int) ChecksumIndexes.Aggregate(ChecksumConstant, (sum, idx) => (byte) (sum + digestBytes[idx]));
        sum = Math.Abs(sum);
        
        // combine the signature parts
        return string.Format(HashFormat, payload, sum);
    }
}