using Telegram.Bot;
using Telegram.Bot.Polling;

namespace OnlineMarket.TelegramBot.Services;

public class BotBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient botClient;
    private readonly IUpdateHandler updateHandler;
    private readonly ILogger<BotBackgroundService> logger;
    public BotBackgroundService(ILogger<BotBackgroundService> logger, ITelegramBotClient botClient, IUpdateHandler updateHandler)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.updateHandler = updateHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = await botClient.GetMeAsync(stoppingToken);

        logger.LogInformation($"Bot started successfully: {bot.Username}", bot.Username);

        botClient.StartReceiving(
        updateHandler.HandleUpdateAsync,
        updateHandler.HandlePollingErrorAsync,
        null,
        stoppingToken);

        await botClient.DeleteWebhookAsync(cancellationToken: stoppingToken);
    }
}
