namespace AnotherChecklistBot.Configurations;

public static class ConfigurationExtensions
{
    public static T GetConfiguration<T>(this IConfiguration configuration) where T : class
    {
        var section = typeof(T).Name;
        return configuration.GetSection(section).Get<T>()
            ?? throw new InvalidOperationException($"{section} configuration is required but was not provided.");
    }
}