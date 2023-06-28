using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Ruby.Api.Auth;

public class TokenAuthenticationHandler : SignInAuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    public TokenAuthenticationHandler(
        IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }
    
    private string? GenerateToken(ClaimsPrincipal user)
    {
        var id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var age = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
        
        if (string.IsNullOrEmpty(id) 
            || string.IsNullOrEmpty(age) 
            || !ulong.TryParse(id, out var parsedId) 
            || !int.TryParse(age, out var parsedAge))
            return null;

        var token = new Token(parsedId, parsedAge);
        
        return token.Generate(Options.SecretKey);
    }
    
    private Token? ExtractTokenFromRequest()
    {
        var token = Request.Headers[TokenAuthenticationDefaults.CookieName].FirstOrDefault();

        if (string.IsNullOrEmpty(token))
            token = Request.Cookies[TokenAuthenticationDefaults.CookieName];

        if (!Token.TryParse(token, Options.SecretKey, out var parsedToken))
            return null;
        
        return parsedToken;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Token? token = ExtractTokenFromRequest();
        
        if (token is null)
            return Task.FromResult(AuthenticateResult.Fail("bad token"));

        // var user = await IUserService.GetUserByIdAsync(token.Id);
        // var claims = user.Claims;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, token.Id.ToString()),
            new(ClaimTypes.UserData, token.Age.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, TokenAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(principal, TokenAuthenticationDefaults.AuthenticationScheme);

        Context.User = principal;

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        // get the user somewhere
        // and add it to the ItemBag
        // Context.Items.Add("user", user);
        
        var token = GenerateToken(user);

        if (string.IsNullOrEmpty(token))
            return Task.CompletedTask;

        Response.Cookies.Append(TokenAuthenticationDefaults.CookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        if (properties is {RedirectUri: var redirect} && !string.IsNullOrEmpty(redirect))
        {
            Response.Redirect(redirect);
        }

        return Task.CompletedTask; 
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        Response.Cookies.Delete(TokenAuthenticationDefaults.CookieName);
        return Task.CompletedTask;
    }
}