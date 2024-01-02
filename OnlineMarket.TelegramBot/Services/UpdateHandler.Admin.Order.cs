using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using OnlineMarket.TelegramBot.Models.Enums;
using OnlineMarket.Domain.Entities.Orders;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private async Task HandlerForOrderAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerForOrderAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Buyurtmani o'chirish"), new KeyboardButton("Ma'lum bir foydalanuvchi buyurtmalari") },
            new[] { new KeyboardButton("Barcha buyurtmalar ro'yxati") },
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

        adminStates[message.Chat.Id] = AdminState.ManageOrderPage;
    }

    private async Task GetAllOrdersAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("GetAllOrdersAsync is working..");

        var orders = await this.orderService.GetAllAsync();
        if (orders.Count() == 0)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Hali bironta ham buyurtma yo'q",
                cancellationToken: cancellationToken);

            await HandleOrderAsync(message, cancellationToken);
        }
        else
        {

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
        new KeyboardButton("🏠 Asosiy menu"),
    })
            {
                ResizeKeyboard = true
            };

            foreach (var order in orders)
            {
                var user = order.User;
                var orderItems = order.Items;

                // Buyurtma haqida ma'lumot
                var orderInfo = $"Buyurtma №{order.Id}\n" +
                                $"Ism: {user.FullName}\n" +
                                $"Telefon: {user.Phone}\n" +
                                $"Manzil: {order.MarketAddress}{order.DeliveryAddress}\n" +
                                $"Xaridor toifasi: {order.OrderType}\n" +
                                $"To'lov usuli: {order.PaymentMethod}\n";

                // OrderItem lar haqida ma'lumot
                var orderItemsInfo = string.Join("\n", orderItems.Select(item =>
                    $"{item.Product.Name} - {item.Quantity} x {item.Price:C} = {item.Sum:C}")
                );

                var fullOrderInfo = $"{orderInfo}\n\nMahsulotlar:\n{orderItemsInfo}";

                // Foydalanuvchiga xabar yuborish
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: fullOrderInfo,
                    replyMarkup: replyKeyboard,
                    cancellationToken: cancellationToken
                );
            }
        }
    }

    private async Task GetAllOrderByUserAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("GetAllOrderByUserAsync is working..");

        var query = message.Text;
        var userOrders = await this.orderService.GetByUserAsync(query);
        
        if (userOrders is not null)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
        new KeyboardButton("🏠 Asosiy menu"),
    })
            {
                ResizeKeyboard = true
            };

            foreach (var order in userOrders)
            {
                var user = order.User;
                var orderItems = order.Items;

                var orderInfo = $"Buyurtma №{order.Id}\n" +
                                $"Ism: {user.FullName}\n" +
                                $"Telefon: {user.Phone}\n" +
                                $"Filial: {order.MarketAddress}\n" +
                                $"Manzil: {order.DeliveryAddress}\n" +
                                $"Buyurtma turi: {order.OrderType}\n" +
                                $"To'lov usuli: {order.PaymentMethod}\n";

                var orderItemsInfo = string.Join("\n", orderItems.Select(item =>
                    $"{item.Product.Name} - {item.Quantity} x {item.Price:C} = {item.Sum:C}")
                );

                var fullOrderInfo = $"{orderInfo}\n\nMahsulotlar:\n{orderItemsInfo}";

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: fullOrderInfo,
                    replyMarkup: replyKeyboard,
                    cancellationToken: cancellationToken
                );
            }
        }
    }

    private async Task HandleOrderIdForDeleteAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleOrderIdForDeleteAsync is working..");

        try
        {
            long orderId = long.Parse(message.Text);
            var result = await this.orderService.DeleteAsync(orderId);
            if (result)
            {
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Buyurtma muvaffaqiyatli o'chirildi!",
                cancellationToken: cancellationToken);

                await HandlerForOrderAsync(message, cancellationToken);
            }
            else
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Qandaydir xatolik kelib chiqdi! Qaytadan urinib ko'ring!",
                cancellationToken: cancellationToken);
        }
        catch
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Raqamni to'g'ri kiriting:",
                cancellationToken: cancellationToken);
        }
    }
}
