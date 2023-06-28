using Microsoft.AspNetCore.Authentication;

namespace Ruby.Api.Auth;

public static class TokenAuthenticationExtensions
{
    public static int GetAge(this DateTime date)
    {
        TimeSpan ts = date - new DateTime(1970, 1, 1);
        return (int) ts.TotalSeconds;
    }

    public static DateTime ToDateTime(this int age)
    {
        return new DateTime(1970, 1, 1).AddSeconds(age);
    }
    
    public static AuthenticationBuilder AddToken(this AuthenticationBuilder builder)
    {
        return builder.AddToken(TokenAuthenticationDefaults.AuthenticationScheme, _ => { });
    }
    
    public static AuthenticationBuilder AddToken(this AuthenticationBuilder builder, string authScheme)
    {
        return builder.AddToken(authScheme, _ => { });
    }
    
    public static AuthenticationBuilder AddToken(this AuthenticationBuilder builder, string authScheme, Action<TokenAuthenticationSchemeOptions> options)
    {
        return builder.AddScheme<TokenAuthenticationSchemeOptions, TokenAuthenticationHandler>(authScheme, options);
    }
}