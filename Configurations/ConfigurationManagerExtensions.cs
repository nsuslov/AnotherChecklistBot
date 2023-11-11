namespace AnotherChecklistBot.Configurations;

public static class ConfigurationManagerExtensions
{
    public static T GetConfiguration<T>(this ConfigurationManager configurationManager) where T : class
    {
        var section = typeof(T).Name;
        return configurationManager.GetSection(section).Get<T>()
            ?? throw new InvalidOperationException($"{section} configuration is required but was not provided.");
    }
}