using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ruby.Common.PrimitiveExt;

namespace Application;

public abstract class ConfigurationBase
{
    public abstract void ConfigureServices(IServiceCollection services);

    protected static bool IsDevelopment => "ASPNETCORE_ENVIRONMENT".FromEnv("Development") == "Development";

    /// <summary>
    /// Configures the configurations from all the assemblies and configuration types.
    /// </summary>
    public static void ConfigureServicesFromAssemblies(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ConfigurationBase).IsAssignableFrom(type))
            .Where(type => type is { IsInterface: false, IsAbstract: false })
            .Select(type => (ConfigurationBase)Activator.CreateInstance(type)!)
            .ToList()
            .ForEach(hostingStartup =>
            {
                var name = hostingStartup.GetType().Name.Replace("Configure", "");
                Console.WriteLine($"[{DateTime.UtcNow:s}.000 INF] Configured {name}");
                hostingStartup.ConfigureServices(services);
            });
    }
}