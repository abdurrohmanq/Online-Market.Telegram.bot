using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using OnlineMarket.TelegramBot.Models.Enums;
using OnlineMarket.Service.DTOs.Categories;
using OnlineMarket.Service.DTOs.Filials;
using OnlineMarket.Domain.Enums;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Entities.Users;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private async Task HandlerForFilialAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerForFilialAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Yangi filial qo'shish"),  new KeyboardButton("Filialni o'chirish") },
            new[] { new KeyboardButton("Filialni yangilash"),  new KeyboardButton("Barcha filiallar ro'yxati") },
            new[] { new KeyboardButton("🏠 Asosiy menu") },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Tanlovingizni belgilang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.ManageFilialPage;
    }

    private async Task HandleFilialForCreateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCategoryNameForCreateAsync is working..");

        var location = message.Text;
        var newLocation = new FilialCreationDto { Location = location };

        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton("🏠 Asosiy menu"),
        })
        {
            ResizeKeyboard = true
        };

        await this.filialService.AddAsync(newLocation);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Filial muvaffaqiyatli yaratildi!",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        await HandlerForFilialAsync(message, cancellationToken: cancellationToken);
    }

    private async Task GetAllFilialAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("GetAllFilialAsync is working...");

        var branches = await this.filialService.GetAllAsync();


        var additionalButtons = new List<KeyboardButton>()
            {
                new KeyboardButton("🏠 Asosiy menu"),
            };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var item in branches)
        {
            var button = new KeyboardButton(item.Location);
            rowButtons.Add(button);

            if (rowButtons.Count == 2)
            {
                allButtons.Add(rowButtons.ToArray());
                rowButtons.Clear();
            }
        }

        if (rowButtons.Any())
        {
            allButtons.Add(rowButtons.ToArray());
        }

        allButtons.Add(additionalButtons.ToArray());

        var replyKeyboard = new ReplyKeyboardMarkup(allButtons) { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Filialni tanlang:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private Dictionary<long, long> filialId = new Dictionary<long, long>();
    private async Task HandleSelectFilialForUpdateAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("GetAllFilialAsync is working...");
        var filial = message.Text;
        var existFilial = await this.filialService.GetByLocationAsync(filial);
        if (existFilial is not null)
        {
            filialId[message.Chat.Id] = existFilial.Id;

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Filialni yangi nomini kiriting:",
            cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdatingFilialName;
        }
        else
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Bunday filial yo'q",
            cancellationToken: cancellationToken);
    }

    private async Task ShowAllFilialAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("ShowAllFilialAsync is working...");

        var branches = await this.filialService.GetAllAsync();

        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton("🏠 Asosiy menu"),
            new KeyboardButton("Yangi filial qo'shish"),
        })
        {
            ResizeKeyboard = true
        };

        foreach (var branch in branches)
        {
            var branchInfo = $"Filial №{branch.Id}\n" +
                             $"Manzil: {branch.Location}\n";

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: branchInfo,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);
        }
    }

    private async Task HandleUpdateFilialNameAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateFilialNameAsync is working...");

        var filialName = message.Text;
        var updateFilial = new FilialCustomDto()
        {
            Id = filialId[message.Chat.Id],
            Location = filialName
        };

        await this.filialService.UpdateAsync(updateFilial);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Filial nomi '{filialName} ga o'zgardi'",
            cancellationToken: cancellationToken);

        await HandlerForFilialAsync(message, cancellationToken: cancellationToken);
    }

    private async Task HandleFilialForDeleteAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleFilialForDeleteAsync is working...");

        var filialName = message.Text;
        var existFilial = await this.filialService.GetByLocationAsync(filialName);
        if(existFilial is not null)
        {
            var result = await this.filialService.DeleteAsync(existFilial.Id);
            if (result)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"{filialName} muvaffaqiyatli o'chirildi!",
                    cancellationToken: cancellationToken);

                await HandlerForFilialAsync(message, cancellationToken: cancellationToken);
            }
            else
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Qandaydir xatolik yuz berdi! Qaytadan urinib ko'ring!",
                cancellationToken: cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Bunday filial yo'q",
                cancellationToken: cancellationToken);
    }
}
