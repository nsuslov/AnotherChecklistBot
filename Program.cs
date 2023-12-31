using Telegram.Bot;
using Telegram.Bot.Polling;
using AnotherChecklistBot.Configurations;
using AnotherChecklistBot.Services.UpdateHandler;
using AnotherChecklistBot.Data.Context;
using AnotherChecklistBot.Services.MessageHandler;
using AnotherChecklistBot.Services.ReceiverService;
using AnotherChecklistBot.Services.CommandHandler;
using AnotherChecklistBot.Services.CallbackQueryHandler;
using AnotherChecklistBot.Services.MessageSender;
using AnotherChecklistBot.Data.Repositories;
using AnotherChecklistBot.Services.ChecklistService;
using AnotherChecklistBot.Services.MessageBuilder;
using AnotherChecklistBot.Services.CommandService;
using AnotherChecklistBot.Services.CommandRegistrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Path.Combine(builder.Environment.ContentRootPath, "config"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IListItemRepository, ListItemRepository>();
builder.Services.AddScoped<IChecklistMessageRepository, ChecklistMessageRepository>();
builder.Services
    .AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
    {
        var configuration = builder.Configuration.GetConfiguration<AnotherChecklistBot.Configurations.Telegram>();
        var options = new TelegramBotClientOptions(configuration.Token);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
builder.Services.AddSingleton<IMessageSender, QueueMessageSender>();
builder.Services.AddSingleton<ICommandService, CommandService>();
builder.Services.AddScoped<IMessageHandler, MessageHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddScoped<ICallbackQueryHandler, CallbackQueryHandler>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddSingleton<IMessageBuilder, MessageBuilder>();

builder.Services.AddHostedService<ReceiverService>();
builder.Services.AddHostedService<CommandRegistrationService>();

var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Migrate();
}
host.Run();
