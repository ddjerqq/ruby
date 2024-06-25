using System.ComponentModel;
using Application;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ConfigureAuth : ConfigurationBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = Jwt.Events;
                options.Audience = Jwt.ClaimsAudience;
                options.ClaimsIssuer = Jwt.ClaimsIssuer;
                options.TokenValidationParameters = Jwt.TokenValidationParameters;
            });

        // an example, of an authorization policy
        services.AddAuthorizationBuilder()
            .AddDefaultPolicy("default", policy => policy.RequireAuthenticatedUser())
            .AddPolicy("is_elon", policy => policy.RequireClaim(JwtRegisteredClaimNames.Name, "elon"));
    }
}