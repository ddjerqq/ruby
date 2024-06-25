using System.ComponentModel;
using DSharpPlus.Commands;

namespace DiscordClient.Cogs;

public sealed class BasicCommandHandler
{
    [Command("ping")]
    [Description("Replies with pong")]
    public static async Task Ping(CommandContext context)
    {
        await context.RespondAsync("Pong!");

        // await context.Channel.TriggerTypingAsync();

        // await context.Channel.SendMessageAsync(new DiscordMessageBuilder()
        //     .WithContent($"Step by step result for {input}")
        //     .AddFile($"{input}_step_by_step.pdf", fsStepByStep, AddFileOptions.CopyStream));
    }
}