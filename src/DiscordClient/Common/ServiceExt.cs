using System.Reflection;
using DiscordClient.Events;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordClient.Common;

public static class ServiceExt
{
    public static async Task<DSharpPlus.DiscordClient> UseCommands(this IServiceProvider services)
    {
        var client = services.GetRequiredService<DSharpPlus.DiscordClient>();

        client.UseInteractivity(new InteractivityConfiguration
        {
            PaginationBehaviour = PaginationBehaviour.Ignore,
        });

        var commandsExtension = client.UseCommands(new CommandsConfiguration
        {
            UseDefaultCommandErrorHandler = false,
            DebugGuildId = ulong.Parse(Util.GetRequiredEnvVariable("DISCORD__DEBUG_GUILD_ID")),
        });

        commandsExtension.AddChecks(Assembly.GetExecutingAssembly());
        commandsExtension.AddParameterChecks(Assembly.GetExecutingAssembly());
        commandsExtension.AddCommands(Assembly.GetExecutingAssembly());

        await commandsExtension.AddProcessorsAsync(new TextCommandProcessor(new TextCommandConfiguration
        {
            IgnoreBots = true,
            PrefixResolver = new DefaultPrefixResolver(true, "!").ResolvePrefixAsync,
        }));

        commandsExtension.CommandErrored += DiscordEventHandler.CommandErrored;

        return client;
    }
}