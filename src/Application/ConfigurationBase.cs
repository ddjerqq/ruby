using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public abstract class ConfigurationBase : IHostingStartup
{
    private bool Configured { get; set; }

    protected abstract void ConfigureServices(WebHostBuilderContext context, IServiceCollection services);

    public virtual void Configure(IWebHostBuilder builder)
    {
        if (!Configured)
        {
            builder.ConfigureServices(ConfigureServices);
            Configured = true;
        }
    }
}