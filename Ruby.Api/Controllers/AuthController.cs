using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ruby.Api.Auth;

namespace Ruby.Api.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController : Controller
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] long id)
    {
        // TODO rename these claims to snowflake and token_ts
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.UserData, DateTime.UtcNow.GetAge().ToString()),
        };
        var identity = new ClaimsIdentity(claims, TokenAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(TokenAuthenticationDefaults.AuthenticationScheme, principal);
        
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(TokenAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
    
    [HttpGet("me")]
    public IActionResult Me()
    {
        var claimData = HttpContext.User.Claims.Select(x => new
        {
            x.Type,
            x.Value,
        });

        return Ok(claimData);
    } 
}