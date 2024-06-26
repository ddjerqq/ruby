using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordClient.Cogs;

public sealed class Basic
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

    [Command("pagination-test")]
    [Description("Test pagination")]
    public static async Task PaginationTest(CommandContext context)
    {
        await context.DeferResponseAsync();

        Page[] pages =
        [
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 1")),
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 2")),
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 3")),
        ];

        await context.Channel.SendPaginatedMessageAsync(context.User, pages);
    }

    // get response
    // await ctx.RespondAsync("Respond with *confirm* to continue.");
    // var result = await ctx.Message.GetNextMessageAsync(m => { return m.Content.ToLower() == "confirm"; });
    //
    // if (!result.TimedOut) await ctx.RespondAsync("Action confirmed.");
}