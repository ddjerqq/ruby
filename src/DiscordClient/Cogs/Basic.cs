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
    public static async Task Ping(CommandContext ctx)
    {
        await ctx.RespondAsync("Pong!");

        // await ctx.Channel.TriggerTypingAsync();

        // await ctx.Channel.SendMessageAsync(new DiscordMessageBuilder()
        //     .WithContent($"Step by step result for {input}")
        //     .AddFile($"{input}_step_by_step.pdf", fsStepByStep, AddFileOptions.CopyStream));
    }

    [Command("pagination-test")]
    [Description("Test pagination")]
    public static async Task PaginationTest(CommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        Page[] pages =
        [
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 1")),
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 2")),
            new Page(embed: new DiscordEmbedBuilder().WithDescription("Page 3")),
        ];

        await ctx.Channel.SendPaginatedMessageAsync(ctx.User, pages);
    }

    [Command("my-cases")]
    [Description("list all your cases")]
    public static async Task MyCases(CommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        var inter = ctx.Client.GetInteractivity();

        var embed = new DiscordEmbedBuilder()
            .WithTitle("Your cases")
            .WithColor(DiscordColor.Green)
            .AddField("Name", "Brokie 50-50")
            .WithImageUrl("https://qu.ax/siuB.webp")
            .WithFooter("Case 1 of 10");

        var previousButton = new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            "case-inventory-previous",
            string.Empty,
            false,
            new DiscordComponentEmoji("⬅️"));

        var openButton = new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            "case-inventory-open",
            string.Empty,
            false,
            new DiscordComponentEmoji("🔓"));

        var nextButton = new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            "case-inventory-next",
            string.Empty,
            false,
            new DiscordComponentEmoji("➡️"));

        var responseMessageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed);

        await ctx.RespondAsync(responseMessageBuilder.AddComponents(previousButton, openButton, nextButton));
        var message = (await ctx.GetResponseAsync())!;

        var result = await inter.WaitForButtonAsync(message);

        if (result.TimedOut)
        {
            await ctx.FollowupAsync("Timed out.");

            // disable buttons
            previousButton.Disable();
            openButton.Disable();
            nextButton.Disable();

            await message.ModifyAsync(responseMessageBuilder.AddComponents(previousButton, openButton, nextButton));
        }
        else
        {
            switch (result.Result.Id)
            {
                case "case-inventory-previous":
                    await ctx.RespondAsync("Previous", embed);
                    break;
                case "case-inventory-open":
                    await ctx.RespondAsync("Open", embed);
                    break;
                case "case-inventory-next":
                    await ctx.RespondAsync("Next", embed);
                    break;
            }
        }
    }
}