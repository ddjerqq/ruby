using DSharpPlus.Commands;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Serilog;

namespace DiscordClient.Events;

public static class DiscordEventHandler
{
    public static Task HandleMessage(DSharpPlus.DiscordClient sender, MessageCreatedEventArgs args)
    {
        Log.Logger.Information("Message received from {Author} in {Channel} {Message} ", args.Author.ToString(), args.Channel.ToString(), args.Message.Content);
        return Task.CompletedTask;
    }

    public static async Task CommandErrored(CommandsExtension sender, CommandErroredEventArgs args)
    {
        if (args.Exception is CommandNotFoundException)
        {
            Log.Logger.Error(args.Exception, "command not found");
            return;
        }

        // TODO better scope incident IDs
        var incidentId = Ulid.NewUlid();

        await args.Context.RespondAsync(new DiscordEmbedBuilder()
            .WithTitle("Something went wrong!")
            .WithColor(DiscordColor.Red)
            .WithDescription($"```js\n{args.Exception.Message}```")
            .WithUrl("https://github.com/ddjerqq/ruby/issues")
            .WithFooter($"Incident id: {incidentId}")
        );

        Log.Logger.Error(args.Exception, $"Incident {incidentId} has occured while executing command {args.Context.Command.Name}");

        throw new Exception($"Incident {incidentId} has occured while executing command {args.Context.Command.Name}", args.Exception);
    }
}