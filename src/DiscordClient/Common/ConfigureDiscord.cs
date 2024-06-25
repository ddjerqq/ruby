using System.ComponentModel;
using Application;
using DiscordClient.Events;
using DSharpPlus;
using DSharpPlus.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordClient.Common;

/// <inheritdoc />
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ConfigureDiscord : ConfigurationBase
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        var token = Util.GetRequiredEnvVariable("DISCORD__BOT_TOKEN");

        services.AddDiscordClient(token,
            DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.DirectMessages | DiscordIntents.GuildMembers | DiscordIntents.GuildPresences);

        services.ConfigureEventHandlers(builder =>
        {
            builder.HandleMessageCreated(DiscordEventHandler.HandleMessage);
        });
    }
}