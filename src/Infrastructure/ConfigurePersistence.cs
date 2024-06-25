using System.ComponentModel;
using Application;
using Application.Services;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Infrastructure;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ConfigurePersistence : ConfigurationBase
{
    protected override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<EntitySaveChangesInterceptor>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddDbContext<AppDbContext>(builder =>
        {
            if (context.HostingEnvironment.IsDevelopment())
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