using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ruby.Api.Auth;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /* [options] */
    public string SecretKey { get; set; } = null!;
}