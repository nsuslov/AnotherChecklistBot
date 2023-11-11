using Telegram.Bot;
using Telegram.Bot.Polling;
using AnotherChecklistBot.ReceiverService;
using AnotherChecklistBot.Configurations;
using AnotherChecklistBot.Services.UpdateHandler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IUpdateHandler, UpdateHandler>();
builder.Services
    .AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
    {
        var configuration = builder.Configuration.GetConfiguration<AnotherChecklistBot.Configurations.Telegram>();
        var options = new TelegramBotClientOptions(configuration.Token);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddHostedService<ReceiverService>();

var host = builder.Build();

host.Run();
