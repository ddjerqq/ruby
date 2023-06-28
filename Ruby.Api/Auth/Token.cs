using System.Security.Cryptography;
using System.Text;

namespace Ruby.Api.Auth;

// TODO need a system for refreshing the token.
// maybe store latest age on server-side
// and if tokens come in with old age, reject them
public class Token
{
    public readonly ulong Id;
    public readonly int Age;

    public DateTime IdCreatedAt => Id.CreatedAt();
    public DateTime TokenCreatedAt => Age.ToDateTime();

    public Token(ulong id, int age)
    {
        Id = id;
        Age = age;
    }

    public static Token CreateToken()
    {
        var id = Snowflake.NewSnowflake();
        var age = DateTime.UtcNow.GetAge();

        return new Token(id, age);
    }
    
    /// <summary>
    /// generates the token string signed with the secretKey
    /// </summary>
    /// <param name="secretKey"></param>
    /// <returns></returns>
    public string Generate(string secretKey)
    {
        var id = Convert.ToBase64String(Encoding.UTF8.GetBytes(Id.ToString())).TrimEnd('=');
        var age = Convert.ToBase64String(BitConverter.GetBytes(Age)).TrimEnd('=');

        var payload = $"{id}.{age}";

        var hashBytes = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))
            .ComputeHash(Encoding.UTF8.GetBytes(payload));

        var hash = Convert.ToBase64String(hashBytes).TrimEnd('=');
        
        return $"{payload}.{hash}";
    }

    /// <summary>
    /// build a token from a string token
    /// validates the token signature
    /// </summary>
    /// <param name="token">the token value</param>
    /// <param name="secretKey">the key used to encrypt the token</param>
    /// <param name="parsedToken">the parsed token</param>
    /// <returns></returns>
    public static bool TryParse(string? token, string secretKey, out Token? parsedToken)
    {
        parsedToken = null;
        
        var parts = token?.Split('.');
        if (parts?.Length != 3)
            return false;

        var head = parts[0];
        var mid = parts[1];
        var sign = parts[2];
        
        // pad the base64 strings
        var paddedId = head.PadRight(head.Length + (4 - head.Length % 4) % 4, '=');
        var paddedAge = mid.PadRight(mid.Length + (4 - mid.Length % 4) % 4, '=');
        
        try
        {
            var id = Encoding.UTF8.GetString(Convert.FromBase64String(paddedId));
            var age = BitConverter.ToInt32(Convert.FromBase64String(paddedAge));

            if (!ulong.TryParse(id, out var parsedId)) return false;

            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))
                .ComputeHash(Encoding.UTF8.GetBytes($"{head}.{mid}"));

            var expectedSignature = Convert.ToBase64String(hash).TrimEnd('=');
            if (sign != expectedSignature) return false;

            parsedToken = new Token(parsedId, age);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool operator ==(Token? token, Token? other)
        => token?.Id == other?.Id && token?.Age == other?.Age;

    public static bool operator !=(Token? token, Token? other) => !(token == other);
    
    protected bool Equals(Token other)
    {
        return Id == other.Id && Age == other.Age;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Token) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Age);
    }

}