using System.Reflection;
using Application;
using dotenv.net;
using Infrastructure;
using Presentation;

// fix postgres timestamp issue
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
DotEnv.Fluent()
    .WithTrimValues()
    .WithEnvFiles($"{solutionDir}/.env")
    .WithOverwriteExistingVars()
    .Load();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseConfiguredSerilog();

builder.WebHost.UseStaticWebAssets();

Assembly[] assemblies =
[
    Domain.Domain.Assembly,
    Application.Application.Assembly,
    Infrastructure.Infrastructure.Assembly,
    Presentation.Presentation.Assembly,
];
ConfigurationBase.ConfigureServicesFromAssemblies(builder.Services, assemblies);

var app = builder.Build();

app.UseConfiguredSerilogRequestLogging();
app.MigrateDatabase();

app.UseRateLimiter();
app.UseCustomHeaderMiddleware();
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
    app.UseDevelopmentMiddleware();

if (app.Environment.IsProduction())
    app.UseProductionMiddleware();

app.UseAppMiddleware();

app.MapEndpoints();

app.Run();