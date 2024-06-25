using DSharpPlus.Commands.ContextChecks.ParameterChecks;

namespace DiscordClient.Checks;


public sealed class MaximumStringLengthAttribute(int length) : ParameterCheckAttribute
{
    public int MaximumLength { get; init; } = length;
}