using Application;
using DiscordClient.Common;
using dotenv.net;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;

DotEnv.Fluent()
    .WithProbeForEnv(6)
    .Load();

var services = new ServiceCollection();
ConfigurationBase.ConfigureServicesFromAssemblies(services, Ruby.Common.Ruby.Assemblies.Append(typeof(ServiceExt).Assembly));

var serviceProvider = services.BuildServiceProvider();
var discordClient = await serviceProvider.UseCommands();

var activity = new DiscordActivity("with your heart", DiscordActivityType.Playing);
await discordClient.ConnectAsync(activity, DiscordUserStatus.Online);
await Task.Delay(-1);