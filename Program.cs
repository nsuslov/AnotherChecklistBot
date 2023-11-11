using Telegram.Bot;
using Telegram.Bot.Polling;
using AnotherChecklistBot.Configurations;
using AnotherChecklistBot.Services.UpdateHandler;
using AnotherChecklistBot.Data.Context;
using AnotherChecklistBot.Services.MessageHandler;
using AnotherChecklistBot.Services.ReceiverService;
using AnotherChecklistBot.Services.CommandHandler;
using AnotherChecklistBot.Services.CallbackQueryHandler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
builder.Services
    .AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
    {
        var configuration = builder.Configuration.GetConfiguration<AnotherChecklistBot.Configurations.Telegram>();
        var options = new TelegramBotClientOptions(configuration.Token);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddScoped<IMessageHandler, MessageHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddScoped<ICallbackQueryHandler, CallbackQueryHandler>();

builder.Services.AddHostedService<ReceiverService>();

var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Migrate();
}

host.Run();
