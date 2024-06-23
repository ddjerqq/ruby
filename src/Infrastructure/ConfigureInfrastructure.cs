using System.ComponentModel;
using Application;
using Application.Services;
using Infrastructure.Idempotency;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ConfigureInfrastructure : ConfigurationBase
{
    protected override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddIdempotency();
        services.AddMemoryCache();

        services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();
        services.AddScoped<IDateTimeProvider, UtcDateTimeProvider>();
        services.AddScoped<IEmailSender, GoogleSmtpEmailSender>();
    }
}