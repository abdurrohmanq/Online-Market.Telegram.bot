using Telegram.Bot;
using Telegram.Bot.Types;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private async Task HandleEditMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;

        logger.LogInformation("Received Edit message from {from.FirstName}: {message.Text}", from?.FirstName, message.Text);
    }
}
