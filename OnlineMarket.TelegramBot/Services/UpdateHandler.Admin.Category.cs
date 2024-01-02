using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Service.DTOs.Categories;
using OnlineMarket.Service.DTOs.Products;
using OnlineMarket.TelegramBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private async Task HandleForManageCategoryAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleForManageCategoryAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Categoriya qo'shish"),  new KeyboardButton("Categoriya o'chirish") },
            new[] { new KeyboardButton("Categoriyani yangilash"),  new KeyboardButton("Barcha categoriyalar ro'yxati") },
            new[] { new KeyboardButton("🏠 Asosiy menu") }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Tanlovingizni belgilang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.ManageCategoryPage;
    }

    private Dictionary<long, CategoryCreationDto> createCategory = new Dictionary<long, CategoryCreationDto>();
    
    private async Task HandleCategoryNameForCreateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCategoryNameForCreateAsync is working..");

        var categoryName = message.Text;
        var newCategory = new CategoryCreationDto { Name = categoryName };

        createCategory[message.Chat.Id] = newCategory;

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
            text: "Categoriya tavsifini kiriting!",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryDescription;
    }

    private async Task HandlerCategoryDescForCreateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerCategoryDescForCreateAsync is working..");

        var categoryDesc = message.Text;
        createCategory[message.Chat.Id].Description = categoryDesc;

        var createdCategory = await this.categoryService.AddAsync(createCategory[message.Chat.Id]);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Categoriya muvaffaqiyatli yaratildi!",
            cancellationToken: cancellationToken);

        if (isFromCreateProduct[message.Chat.Id])
        {
            createProduct[message.Chat.Id].CategoryId = createdCategory.Id;
            await ProductAddAsync(message, cancellationToken);
            isFromCreateProduct[message.Chat.Id] = false;
        }
        else
        {
            await HandleForManageCategoryAsync(message, cancellationToken);
        }
    }

    private async Task HandleSelectCategoryForDeleteAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleSelectCategoryForDeleteAsync is working..");

        var categoryName = message.Text;
        var existingCategory = await this.categoryService.GetByName(categoryName);
        if (existingCategory is null)
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bunday nomli category yo'q",
                cancellationToken: cancellationToken);
        else
        {
            await this.categoryService.DeleteAsync(existingCategory.Id);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Categoriya muvaffaqiyatli o'chirildi!",
                cancellationToken: cancellationToken);
        }
    }

    private Dictionary<long, CategoryUpdateDto> updateCategory = new Dictionary<long, CategoryUpdateDto>();
    private Dictionary<long, bool> isAllUpdateCategory = new Dictionary<long, bool>();
    private Dictionary<long, CategoryResultDto> existCategory = new Dictionary<long, CategoryResultDto>();

    private async Task HandleCategorySelectForUpdateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCategorySelectForUpdateAsync is working..");
        var categoryName = message.Text.Trim();
        existCategory[message.Chat.Id] = await this.categoryService.GetByName(categoryName);

        if (existCategory is null)
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bunday categoriya mavjud emas",
                cancellationToken: cancellationToken);
        else
        {
            var updatingCategory = new CategoryUpdateDto()
            {
                Id = existCategory[message.Chat.Id].Id,
                Name = existCategory[message.Chat.Id].Name,
                Description = existCategory[message.Chat.Id].Description,
            };
            updateCategory[message.Chat.Id] = updatingCategory;

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
            new[] { new KeyboardButton("Categoriya nomi"),  new KeyboardButton("Categoriya tavsifi") },
            new[] { new KeyboardButton("Barchasini"), new KeyboardButton("⬅️ Ortga") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Categoriya nomi: {existCategory[message.Chat.Id].Name}\nCategoriya tavsifi: {existCategory[message.Chat.Id].Description}",
            cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Categoriyani qaysi qismini o'zgartirmoqchisiz?",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateCategoryProperty;
        }
    }

    private async Task HandleSelectCategoryInfoAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCategorySelectForUpdateAsync is working..");
        var categoryName = message.Text.Trim();
        existCategory[message.Chat.Id] = await this.categoryService.GetByName(categoryName);

        if (existCategory[message.Chat.Id] is null)
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bunday categoriya mavjud emas",
                cancellationToken: cancellationToken);

        else
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
            new[] { new KeyboardButton("Categoriyani o'chirish"),  new KeyboardButton("Categoriyani yangilash") },
            new[] { new KeyboardButton("🏠 Asosiy menu"), new KeyboardButton("⬅️ Ortga") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Categoriya nomi: {existCategory[message.Chat.Id].Name}\nCategoriya tavsifi: {existCategory[message.Chat.Id].Description}",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForCategoryInfoSelection;
        }
    }

    private async Task HandleUpdateCategoryNameAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductNameAsync is working..");

        var categoryName = message.Text;
        updateCategory[message.Chat.Id].Name = categoryName;
        if (!isAllUpdateCategory[message.Chat.Id])
        {
            await this.categoryService.UpdateAsync(updateCategory[message.Chat.Id]);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Categoriya nomi"),  new KeyboardButton("Categoriya tavsifi") },
            new[] { new KeyboardButton("⬅️ Ortga") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Categoriyani nomi '{categoryName}' ga o'zgardi!",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateCategoryProperty;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Categoriyani yangi tavsifini kiriting:",
                            cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateCategoryDesc;
        }
    }

    private async Task HandleCategoryDescForUpdateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductNameAsync is working..");

        var categoryDesc = message.Text;
        updateCategory[message.Chat.Id].Description = categoryDesc;
        if (!isAllUpdateCategory[message.Chat.Id])
        {
            await this.categoryService.UpdateAsync(updateCategory[message.Chat.Id]);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Categoriya nomi"),  new KeyboardButton("Categoriya tavsifi") },
            new[] { new KeyboardButton("⬅️ Ortga") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Categoriyani nomi '{categoryDesc}' ga o'zgardi!",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateCategoryProperty;
        }
        else
        {
            await this.categoryService.UpdateAsync(updateCategory[message.Chat.Id]);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Categoriya xususiyatlari yangilandi!",
                cancellationToken: cancellationToken);

            isAllUpdateCategory[message.Chat.Id] = false;
            await HandleForManageCategoryAsync(message, cancellationToken);
        }
    }
}
