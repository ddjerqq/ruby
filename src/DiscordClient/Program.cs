using System.Reflection;
using Application;
using DiscordClient;
using dotenv.net;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;

DotEnv.Fluent()
    .WithProbeForEnv(6)
    .Load();

Assembly[] assemblies =
[
    Domain.Domain.Assembly,
    Application.Application.Assembly,
    Infrastructure.Infrastructure.Assembly,
    typeof(ServiceExt).Assembly,
];

var services = new ServiceCollection();
ConfigurationBase.ConfigureServicesFromAssemblies(services, assemblies);

var serviceProvider = services.BuildServiceProvider();
var discordClient = await serviceProvider.UseCommands();

var activity = new DiscordActivity("with your heart", DiscordActivityType.Playing);
await discordClient.ConnectAsync(activity, DiscordUserStatus.Online);
await Task.Delay(-1);