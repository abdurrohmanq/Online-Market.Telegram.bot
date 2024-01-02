using Telegram.Bot;
using Telegram.Bot.Types;
using OnlineMarket.Domain.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using OnlineMarket.Service.DTOs.Users;
using OnlineMarket.TelegramBot.Models.Enums;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private Dictionary<long, AdminState> adminStates = new Dictionary<long, AdminState>();

    bool isCategory = false;
    private Dictionary<long, bool> isDelete = new Dictionary<long, bool>();
    private Dictionary<long, bool> allUpdate = new Dictionary<long, bool>();
    private Dictionary<long, bool> isGetAllFilial = new Dictionary<long, bool>();
    private Dictionary<long, bool> isFromCreateProduct = new Dictionary<long, bool>();
    private Dictionary<long, UserResultDto> existAdmin = new Dictionary<long, UserResultDto>();

    private async Task HandleTextForAdminMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var admin = existAdmin[message.Chat.Id] = await this.userService.GetByChatId(message.Chat.Id);
        var adminState = adminStates.TryGetValue(message.Chat.Id, out var state) ? state : AdminState.None;
        
        if (!allUpdate.ContainsKey(message.Chat.Id))
             allUpdate[message.Chat.Id] = false;
        
        if (!isDelete.ContainsKey(message.Chat.Id))
             isDelete[message.Chat.Id] = false;

        if (!isGetAllFilial.ContainsKey(message.Chat.Id))
            isGetAllFilial[message.Chat.Id] = false;

        if (!isFromCreateProduct.ContainsKey(message.Chat.Id))
            isFromCreateProduct[message.Chat.Id] = false;

        if(!isAllUpdateCategory.ContainsKey(message.Chat.Id))
            isAllUpdateCategory[message.Chat.Id] = false;

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
                switch (message.Text)
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

                    case "Categoriyalarni boshqarish":
                        await HandleForManageCategoryAsync(message, cancellationToken);
                        break;
                }
                break;

            //Order
            case AdminState.ManageOrderPage:
                switch(message.Text)
                {
                    case "Barcha buyurtmalar ro'yxati":
                        await GetAllOrdersAsync(message, cancellationToken);
                        break;

                    case "Buyurtmani o'chirish":
                        await GetAllOrdersAsync(message, cancellationToken);
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "O'chirmoqchi buyurtmani raqamini kiriting:",
                            cancellationToken: cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForOrderIdForDelete;
                        break;

                    case "Ma'lum bir foydalanuvchi buyurtmalari":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Foydalanuvchini ma'lum bir ma'lumotini kiriting:\n" +
                            "Masalan: Telefon raqami, ismi-sharifi yoki telegram username",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForEnterUserInfo;
                        break;
                }
                break;

            case AdminState.WaitingForEnterUserInfo:
                try
                {
                    await GetAllOrderByUserAsync(message, cancellationToken);
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
                break;

            case AdminState.WaitingForOrderIdForDelete:
                await HandleOrderIdForDeleteAsync(message, cancellationToken);
                break;

            //FilialPage
            case AdminState.ManageFilialPage:
                switch (message.Text)
                {
                    case "Yangi filial qo'shish":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Filial nomini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCreatingFilialEnter;
                        break;

                    case "Filialni yangilash":
                        await GetAllFilialAsync(message, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdatingFilialEnter;
                        break;

                    case "Filialni o'chirish":
                        await GetAllFilialAsync(message, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForHandleDeleteFilial;
                        break;

                    case "Barcha filiallar ro'yxati":
                        await ShowAllFilialAsync(message, cancellationToken);

                        adminStates[message.Chat.Id] = AdminState.WaitingForGetAllFilial;
                        break;
                }
                break;

            case AdminState.WaitingForGetAllFilial:
                switch (message.Text) 
                {
                    case "Yangi filial qo'shish":
                        await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: "Filial nomini kiriting:",
                                    cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCreatingFilialEnter; 
                        break;
                }
                break;

            case AdminState.WaitingForHandleDeleteFilial:
                await HandleFilialForDeleteAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForCreatingFilialEnter:
                await HandleFilialForCreateAsync(message, cancellationToken);   
                break;

            case AdminState.WaitingForUpdatingFilialEnter:
                await HandleSelectFilialForUpdateAsync(message, cancellationToken);
                break;

            case AdminState.WaitingForUpdatingFilialName:
                await HandleUpdateFilialNameAsync(message, cancellationToken);
                break;

            //Category Page
            case AdminState.ManageCategoryPage:
                switch(message.Text)
                {
                    case "Categoriya qo'shish":
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
                            text: "Categoriya nomini kiriting!",
                            replyMarkup: keyboard,
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryNameEnter;
                        break;

                    case "Categoriya o'chirish":
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelectionForDelete;
                        break;

                    case "Categoriyani yangilash":
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingCategorySelectForUpdate;
                        break;

                    case "Barcha categoriyalar ro'yxati":
                        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.HandleSelectCategoryForGet;
                        break;
                }
                break;

            case AdminState.HandleSelectCategoryForGet:
                await HandleSelectCategoryInfoAsync(message, cancellationToken); 
                break;

            case AdminState.WaitingForCategoryInfoSelection:
                switch(message.Text)
                {
                    case "Categoriyani o'chirish":
                        message.Text = existCategory[message.Chat.Id].Name;
                        await HandleSelectCategoryForDeleteAsync(message, cancellationToken);
                        break;
                    case "Categoriyani yangilash":
                        message.Text = existCategory[message.Chat.Id].Name;
                        await HandleCategorySelectForUpdateAsync(message, cancellationToken); 
                        break;
                    case "⬅️ Ortga":
                        await HandleForManageCategoryAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingCategorySelectForUpdate:
                await HandleCategorySelectForUpdateAsync(message, cancellationToken); 
                break;

            case AdminState.WaitingForUpdateCategoryProperty:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        await HandleForManageCategoryAsync(message, cancellationToken);
                        break;

                    case "Categoriya nomi":
                        isAllUpdateCategory[message.Chat.Id] = false;
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
                           text: "Categoriya yangi nomini kiriting",
                           replyMarkup: keyboard,
                           cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryNameForUpdate;
                        break;
                    case "Categoriya tavsifi":
                    isAllUpdateCategory[message.Chat.Id] = false;

                        var keyboard2 = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton("🏠 Asosiy menu"),
                            new KeyboardButton("⬅️ Ortga")
                       })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Categoriya yangi tavsifini kiriting",
                            replyMarkup: keyboard2,
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateCategoryDesc;
                        break;

                    case "Barchasini":
                        isAllUpdateCategory[message.Chat.Id] = true;
                        var keyboard3 = new ReplyKeyboardMarkup(new[]
                       {
                            new KeyboardButton("🏠 Asosiy menu"),
                            new KeyboardButton("⬅️ Ortga")
                       })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Categoriya yangi nomini kiriting",
                            replyMarkup: keyboard3,
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryNameForUpdate;
                        break;
                }
                break;

            case AdminState.WaitingForCategoryNameForUpdate:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        await HandleCategorySelectForUpdateAsync(message, cancellationToken);
                        break;
                    default:
                        await HandleUpdateCategoryNameAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForUpdateCategoryDesc:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        message.Text = existCategory[message.Chat.Id].Name;
                        await HandleCategorySelectForUpdateAsync(message, cancellationToken);
                        break;
                    default:
                        await HandleCategoryDescForUpdateAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForCategorySelectionForDelete:
                switch(message.Text)
                {
                    case "⬅️ Ortga":
                        await HandleForManageCategoryAsync(message, cancellationToken);
                        break;
                    default:
                        await HandleSelectCategoryForDeleteAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForCategoryNameEnter:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        if (!isFromCreateProduct[message.Chat.Id])
                             await HandleForManageCategoryAsync(message, cancellationToken);
                        else
                        {
                            await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                            adminStates[message.Chat.Id] = AdminState.WaitingForCategoryId;
                        }
                        break;

                    default:
                        await HandleCategoryNameForCreateAsync(message, cancellationToken);
                        break;
                }
                break;

            case AdminState.WaitingForCategoryDescription:
                switch (message.Text)
                {
                    case "⬅️ Ortga":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Categoriya nomini kiriting!",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryNameEnter;
                        break;
                    default:
                        await HandlerCategoryDescForCreateAsync(message, cancellationToken);
                        break;
                }
                break;
            

            //Products for cases
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
                        allUpdate[message.Chat.Id] = false;
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi nomini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductName;
                        break;

                    case "Mahsulot tavsifi":
                        allUpdate[message.Chat.Id] = false;
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi tavsifini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductDesc;
                        break;

                    case "Mahsulotni narxi":
                        allUpdate[message.Chat.Id] = false;
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi narxini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductPrice;
                        break;

                    case "Mahsulor miqdorini":
                        allUpdate[message.Chat.Id] = false;
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi miqdorini kiriting:",
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductQuantity;
                        break;

                    case "Mahsulot categoriyasini":
                        allUpdate[message.Chat.Id] = false;
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
                if (isCategory)
                {
                    if (message.Text == "⬅️ Ortga")
                    {
                        await DisplayGetAllProductAsync(message.Chat.Id, cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForSelectProduct;
                        isCategory = false;
                        break;
                    }
                    else
                    {
                            await HandleCategorySelectionForAdminAsync(message, cancellationToken);
                            isCategory = false;
                    }
                    
                }

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
                    case "Yangi yaratish":
                        isFromCreateProduct[message.Chat.Id] = true;
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
                            text: "Yangi categoriya nomini kiriting!",
                            replyMarkup: keyboard,
                            cancellationToken: cancellationToken);
                        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryNameEnter;
                        break;

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
