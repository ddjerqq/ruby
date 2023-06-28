using System.Diagnostics;
using Ruby.Api.Auth;

namespace Ruby.Test;

public class TestToken
{
    [Test]
    public void TestSnowflake()
    {
        var id = Snowflake.NewSnowflake();
        
        Console.WriteLine(id);
        
        Debug.Assert(id.CreatedAt() - DateTime.UtcNow < TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestCreateToken()
    {
        var token = Token.CreateToken();
        var tokenValue = token.Generate("supersecret");
        
        Console.WriteLine(tokenValue);
        
        Debug.Assert(Token.TryParse(tokenValue, "supersecret", out var parsedToken));
        Debug.Assert(parsedToken != null);

        var parsedTokenValue = parsedToken.Generate("supersecret");
        Console.WriteLine(parsedTokenValue);
        
        Debug.Assert(tokenValue == parsedTokenValue);
    }

    [Test]
    public void QuickNDirty()
    {
        Assert.Pass();
    }
}