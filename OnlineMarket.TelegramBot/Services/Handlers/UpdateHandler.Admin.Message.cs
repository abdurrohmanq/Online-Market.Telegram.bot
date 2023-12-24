using Telegram.Bot;
using Telegram.Bot.Types;
using OnlineMarket.Domain.Enums;
using OnlineMarket.Service.DTOs.Users;
using OnlineMarket.TelegramBot.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private Dictionary<long, AdminState> adminStates = new Dictionary<long, AdminState>();

    bool isCategory = false;
    private Dictionary<long, bool> isDelete = new Dictionary<long, bool>();
    private Dictionary<long, bool> allUpdate = new Dictionary<long, bool>();
    private Dictionary<long, UserResultDto> existAdmin = new Dictionary<long, UserResultDto>();
    private Dictionary<long, List<AdminState>> previousState = new Dictionary<long, List<AdminState>>();

    private async Task HandleTextForAdminMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var admin = existAdmin[message.Chat.Id] = await this.userService.GetByChatId(message.Chat.Id);
        var adminState = adminStates.TryGetValue(message.Chat.Id, out var state) ? state : AdminState.None;
        if (!allUpdate.ContainsKey(message.Chat.Id))
             allUpdate[message.Chat.Id] = false;
        
        if (!isDelete.ContainsKey(message.Chat.Id))
             allUpdate[message.Chat.Id] = false;

        if (message.Text == "🏠 Asosiy menu")
            await DisplayMenuAsync(message, cancellationToken);

        switch (adminState)
        { 
            case AdminState.None:
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Parolni kiriting:",
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForPasswordEnter;
                break;

            case AdminState.WaitingForPasswordEnter:
                var check = await CheckAsync(message, cancellationToken);
                if (check && admin.UserRole == Role.User)
                {
                    await SendMessageToAdminAsync(message, cancellationToken);
                }
                else if (check && admin.UserRole == Role.Admin)
                {
                    await DisplayMenuAsync(message, cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Parol xato kiritildi!",
                        cancellationToken: cancellationToken);
                }
                break;

            case AdminState.WaitingForSelectionMenu:
                switch(message.Text)
                {
                    case "Mahsulotlarni boshqarish":
                        await HandlerForManageProductAsync(message, cancellationToken);
                        break;

                    case "Filiallarni boshqarish":
                        await HandlerForFilialAsync(message, cancellationToken);
                        break;

                    case "Buyurtmalarni boshqarish":
                        await HandlerForOrderAsync(message, cancellationToken);
                        break;

                    case "Parolni o'zgartirish":
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Yangi Parol kiriting:",
                        cancellationToken: cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdatePassword;
                    break;
                }
                break;

            case AdminState.ManageProductPage:
                switch(message.Text)    
                {
                    case "Mahsulot qo'shish":
                        var keyboard = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton("🏠 Asosiy menu"),
                            new KeyboardButton("⬅️ Ortga")
                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulot nomini kiriting!",
                            replyMarkup: keyboard,
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForProductNameEnter; 
                        break;

                    case "Mahsulot o'chirish":
                        isDelete[message.Chat.Id] = true;
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelection;
                        break;

                    case "Mahsulotni yangilash":
                        isUpdate[message.Chat.Id] = true;
                        await DisplayGetAllProductAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForSelectProduct;
                        break;

                    case "Barcha mahsulotlar ro'yxati":
                        isDelete[message.Chat.Id] = false;
                        isUpdate[message.Chat.Id] = false;
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelection;
                        break;
                }
                break;

            case AdminState.WaitingForUpdateProductProperty:
                switch(message.Text)
                {
                    case "Mahsulot nomi":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi nomini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductName;
                        break;

                    case "Mahsulot tavsifi":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi tavsifini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductDesc;
                        break;

                    case "Mahsulotni narxi":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi narxini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductPrice;
                        break;

                    case "Mahsulor miqdorini":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi miqdorini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductQuantity;
                        break;

                    case "Mahsulot categoriyasini":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi categoriyasini kiriting:",
                            cancellationToken: cancellationToken);
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductCategory;
                        break;

                    case "Barchasini":
                        allUpdate[message.Chat.Id] = true;
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi nomini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductName;
                        break;
                }
                break;

            case AdminState.WaitingForUpdateProductName:
                await HandleUpdateProductNameAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForUpdateProductDesc:
                await HandleUpdateProductDescAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForUpdateProductPrice:
                await HandleUpdateProductPriceAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForUpdateProductQuantity:
                await HandleUpdateProductQuantityAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForUpdateProductCategory:
                await HandleUpdateProductCategoryAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForSelectProduct:
                if(isCategory)
                    await HandleCategorySelectionForAdminAsync(message, cancellationToken);
                    isCategory = false;

                switch (message.Text)
                {
                    case "⬅️ Categoriya menu":
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        isCategory = true;
                        break;

                    case "⬅️ Ortga":
                        await HandlerForManageProductAsync(message, cancellationToken);
                        break;

                    default: 
                        await HandleProductSelectionForAdminAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForCategorySelection:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        await HandlerForManageProductAsync(message, cancellationToken);
                        break;
                    default:
                        await HandleCategorySelectionForAdminAsync(message, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForProductSelection;
                        break;
                }
                break;

            case AdminState.WaitingForProductSelection:
                switch(message.Text)
                {
                    case "⬅️ Categoriya menu":
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelection;
                        break;
                    default:
                        if (isDelete[message.Chat.Id])
                            await HandleProductSelectAsync(message, cancellationToken);
                        else
                            await HandleProductSelectionForAdminAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForProductNameEnter:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        await HandlerForManageProductAsync(message, cancellationToken);
                        break;
                    default:
                        await HandleProductNameAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForProductDescription:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulot nomini kiriting!",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForProductNameEnter;
                        break;
                    default:
                        await HandleProductDescAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForProductPrice:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Mahsulot tavsifini kiriting!",
                        cancellationToken: cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForProductDescription;
                        break;
                    default:
                        await HandleProductPriceAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForProductQuantity:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Mahsulot narxini kiriting!",
                        cancellationToken: cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForProductPrice;
                        break;
                    default:
                        await HandleProductQuantityAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForCategoryId:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Mahsulot miqdorini kiriting!",
                        cancellationToken: cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForProductQuantity;
                        break;
                    default:
                        await HandleCategorySelectAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForUpdatePassword:
                await UpdatePasswordAsync(message, cancellationToken);
                break;
        }
    }
}
