using System.Reflection;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DiscordClient;

public static class ServiceExt
{
    public static async Task<DSharpPlus.DiscordClient> UseCommands(this IServiceProvider services)
    {
        var client = services.GetRequiredService<DSharpPlus.DiscordClient>();

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

        commandsExtension.CommandErrored += async (_, e) =>
        {
            if (e.Exception is CommandNotFoundException)
            {
                Log.Logger.Error(e.Exception, "command not found");
                return;
            }

            // TODO better scope incident IDs
            var incidentId = Ulid.NewUlid();

            await e.Context.RespondAsync(new DiscordEmbedBuilder()
                .WithTitle("Something went wrong!")
                .WithColor(DiscordColor.Red)
                .WithDescription($"```js\n{e.Exception.Message}```")
                .WithUrl("https://github.com/ddjerqq/ruby/issues")
                .WithFooter($"Incident id: {incidentId}")
            );

            Log.Logger.Error(e.Exception, $"Incident {incidentId} has occured while executing command {e.Context.Command.Name}");

            throw new Exception($"Incident {incidentId} has occured while executing command {e.Context.Command.Name}", e.Exception);
        };

        return client;
    }
}