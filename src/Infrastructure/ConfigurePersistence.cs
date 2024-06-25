using System.ComponentModel;
using Application;
using Application.Services;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ConfigurePersistence : ConfigurationBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<EntitySaveChangesInterceptor>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddDbContext<AppDbContext>(builder =>
        {
            if (IsDevelopment)
            {
                builder.EnableDetailedErrors();
                builder.EnableSensitiveDataLogging();
            }

            // To see warnings in the console about potentially slow queries.
            // builder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

            var dbPath = "DB__PATH".FromEnv();
            builder.UseSqlite($"Data Source={dbPath}", sqliteOptions =>
            {
                // TODO test if we need this
                // sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        // delegate the IDbContext to the AppDbContext;
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
    }
}