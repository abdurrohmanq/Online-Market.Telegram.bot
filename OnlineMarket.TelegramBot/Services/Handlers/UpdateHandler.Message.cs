using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using OnlineMarket.TelegramBot.Models.Enums;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private async Task HandleMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;

        logger.LogInformation("Received message from {from.FirstName}", from.FirstName);

        var handler = message.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(client, message, cancellationToken),
            MessageType.Contact => HandleContactAsync(message, cancellationToken),
            MessageType.Location => HandleLocationAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(client, message, cancellationToken)
        };

        await handler;
    }

    private Task HandleUnknownMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Received message type: {message.Type}");
        return Task.CompletedTask;
    }

    private Dictionary<long, UserState> userStates = new Dictionary<long, UserState>();

    private async Task HandleTextMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var from = message.From;
        logger.LogInformation("From: {from.FirstName}", from?.FirstName);
        var existUser = await userService.GetByChatId(message.Chat.Id);

        var userState = userStates.TryGetValue(message.Chat.Id, out var state) ? state : UserState.None;

        if (message.Text == "🏠 Asosiy menu")
        {
            await BotOnSendMenuAsync(message, cancellationToken);
            userStates[message.Chat.Id] = UserState.None;
        }
        else if (message.Text == "🛒 Savat")
            await HandleCartAsync(message, cancellationToken);

        else if (message.Text == "🚖 Buyurtma berish")
            await HandleOrderAction(message, cancellationToken);

        else if (message.Text == "⬅️ Categoriya menu")
            await DisplayCategoryKeyboardAsync(message.Chat.Id, cancellationToken);

        else if (message.Text == "⬅️ Product menu")
            await DisplayProductsKeyboardAsync(message.Chat.Id, products[message.Chat.Id], cancellationToken);

        else
        {
            switch (userState)
            {
                case UserState.WaitingForUserFullName:
                    await HandleUserInfoAsync(message, cancellationToken);
                    break;

                case UserState.None:
                    switch (message.Text)
                    {
                        case "/start":
                            if (existUser is null)
                                await BotOnSendMessageAsync(message, cancellationToken);
                            else
                                await BotOnSendMenuAsync(message, cancellationToken);
                            break;

                        case "🛍 Buyurtma berish":
                            await HandleOrderAsync(message, cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForOrderAction;
                            break;

                        case "⚙️ Sozlamalar":
                            await HandleSettingsAsync(message, cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForSettingsAction;
                            break;

                        case "✍️ Fikr bildirish":
                            await ShowFeedbackAsync(message, cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForFeedBack;
                            break;

                        case "☎️ Biz bilan aloqa":
                            await HandleContactWithUs(message, cancellationToken);
                            break;

                        case "ℹ️ Ma'lumot":
                            await HandleInfo(message, cancellationToken);
                            break;

                        default:
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Mahsulot buyurtma bering!",
                                cancellationToken: cancellationToken);
                            break;
                    }
                    break;

                case UserState.WaitingForOrderAction:
                    switch (message.Text)
                    {
                        case "🚖 Yetkazib berish":
                            await HandleDeliveryAsync(message, cancellationToken);
                            break;

                        case "🏃 Olib ketish":
                            await HandleTakeAwayAsync(message, cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForFilialSelection;
                            break;

                        default:
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Iltimos, tanlangan buyurtma amalini bajarish uchun kerakli buyrug'ni tanlang.",
                                cancellationToken: cancellationToken);
                            break;
                    }
                    break;

                case UserState.WaitingForFeedBack:
                    await HandleFeedBackAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForFilialSelection:
                    await HandleFilialSelectionAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForSettingsAction:
                    switch (message.Text)
                    {
                        case "Ism sharifni o'zgartirish":
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Ism sharifingizni kiriting:",
                                cancellationToken: cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForUserFullName;
                            break;

                        case "Raqamni o'zgartirish":
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Telefon raqamingizni kiriting: \n Misol - '+998901234567'",
                                cancellationToken: cancellationToken);
                            userStates[message.Chat.Id] = UserState.WaitingForPhoneNumber;
                            break;

                        default:
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Iltimos, tanlangan sozlamalar amalini bajarish uchun kerakli buyrug'ni tanlang.",
                                cancellationToken: cancellationToken);
                            break;
                    }
                    break;

                case UserState.WaitingForPhoneNumber:
                    await HandlePhoneNumber(message, cancellationToken);
                    break;

                case UserState.WaitingForCategorySelection:
                    await HandleCategorySelectionAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForProductSelection:
                    await HandleProductSelectionAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForQuantityInput:
                    await HandleQuantityInputAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForCartAction:
                    if (message.Text.Contains("❌"))
                        await HandleProductForDeleteAsync(message, cancellationToken);

                    else if (message.Text == "🔄 Tozalash")
                        await HandleCleanCartAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForCommentAction:
                    await HandleDescriptionAsync(message, cancellationToken);
                    break;

                case UserState.WaitingForPaymentTypeAction:
                    await HandlePaymentMethod(message, cancellationToken);
                    break;

                case UserState.WaitingForOrderSaveAction:
                    await HandleSaveOrderAsync(message, cancellationToken);
                    break;

                default:
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Iltimos, amalni bajarish uchun kerakli buyrug'ni tanlang.",
                        cancellationToken: cancellationToken);
                    userStates[message.Chat.Id] = UserState.None;
                    break;
            }
        }
    }
}
