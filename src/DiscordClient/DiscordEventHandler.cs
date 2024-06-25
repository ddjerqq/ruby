using DSharpPlus.EventArgs;
using Serilog;

namespace DiscordClient;

public static class DiscordEventHandler
{
    public static Task HandleMessage(DSharpPlus.DiscordClient sender, MessageCreatedEventArgs args)
    {
        Log.Logger.Information("Message received: {Message}", args.Message.Content);
        return Task.CompletedTask;
    }
}