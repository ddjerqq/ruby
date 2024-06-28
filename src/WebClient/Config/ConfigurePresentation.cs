using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.ResponseCompression;
using Ruby.Common.PrimitiveExt;
using Ruby.Generated;
using WebClient.Filters;
using WebClient.HealthChecks;
using WebClient.JsonConverters;

namespace WebClient.Config;

/// <inheritdoc />
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ConfigurePresentation : ConfigurationBase
{
    private static readonly string[] CompressionTypes = ["application/octet-stream"];

    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAntiforgery();

        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("db")
            .AddCheck<TestHealthCheck>("Test");

        services.AddScoped<GlobalExceptionHandlerMiddleware>();
        services.AddHttpContextAccessor();

        services.Configure<RouteOptions>(x =>
        {
            x.LowercaseUrls = true;
            x.LowercaseQueryStrings = true;
            x.AppendTrailingSlash = false;
        });

        services
            .AddControllers(o =>
            {
                if (IsDevelopment)
                {
                    o.Filters.Add<ResponseTimeFilter>();
                }

                o.Filters.Add<SetClientIpAddressFilter>();
                o.Filters.Add<FluentValidationFilter>();
                o.RespectBrowserAcceptHeader = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // fixes annoying bug with System.Text.Json and EF Core
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                // source generators
                options.JsonSerializerOptions.Converters.Add(new UlidToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new UserIdToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new ItemIdToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new ItemTypeIdToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new CaseIdToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new CaseTypeIdToStringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new OutboxMessageIdToStringJsonConverter());
            });

        services.AddCors(options =>
        {
            var webAppDomain = "WEB_APP__DOMAIN".FromEnvRequired();

            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.WithOrigins(webAppDomain);
                policy.WithMethods("GET", "POST", "HEAD");
                policy.AllowCredentials();
            });
        });

        services.AddSignalR(o => { o.EnableDetailedErrors = IsDevelopment; });

        services.AddResponseCaching();
        services.AddResponseCompression(o =>
        {
            o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(CompressionTypes);
            o.Providers.Add<GzipCompressionProvider>();
            o.Providers.Add<BrotliCompressionProvider>();
        });
    }
}