using System.ComponentModel;
using Application;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Auth;

namespace WebClient.Config;


/// <inheritdoc />
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ConfigureBlazor : ConfigurationBase
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddCascadingAuthenticationState();
        services.AddBlazoredLocalStorage();

        services.AddScoped<AuthenticationStateProvider, HttpContextAuthenticationStateProvider>();
    }
}