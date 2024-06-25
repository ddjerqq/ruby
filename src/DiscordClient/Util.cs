namespace DiscordClient;

public static class Util
{
    public static string GetRequiredEnvVariable(string name) => Environment.GetEnvironmentVariable(name) ?? throw new Exception($"{name} is not set");

    public static string GetRequiredEnvVariable(string name, string @default) => Environment.GetEnvironmentVariable(name) ?? @default;
}