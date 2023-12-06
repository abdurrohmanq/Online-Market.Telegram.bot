using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Enums;
using OnlineMarket.Service.DTOs.Carts;
using OnlineMarket.Service.DTOs.Orders;
using OnlineMarket.Service.DTOs.Products;
using OnlineMarket.Service.DTOs.Users;
using OnlineMarket.Service.Interfaces;
using OnlineMarket.TelegramBot.Models.Enums;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler : IUpdateHandler
{
    private readonly IUserService userService;
    private readonly ICartService cartService;
    private readonly IOrderService orderService;
    private readonly IFilialService filialService;
    private readonly ITelegramBotClient botClient;
    private readonly ILogger<UpdateHandler> logger;
    private readonly IProductService productService;
    private readonly ICategoryService categoryService;
    private readonly ICartItemService cartItemService;
    public UpdateHandler(ILogger<UpdateHandler> logger,
                         ITelegramBotClient botClient,
                         IUserService userService,
                         ICategoryService categoryService,
                         IProductService productService,
                         ICartItemService cartItemService,
                         ICartService cartService,
                         IOrderService orderService,
                         IFilialService filialService)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.userService = userService;
        this.cartService = cartService;
        this.orderService = orderService;
        this.filialService = filialService;
        this.productService = productService;
        this.cartItemService = cartItemService;
        this.categoryService = categoryService;
    }

    private async Task BotOnSendMessageAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("BotOnSendMessageAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("Telefon raqamni jo'natish") {RequestContact = true }
        }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Assalomu alaykum! Botimizga xush kelibsiz. Iltimos, telefon raqamingizni jo'natish uchun quyidagi tugmani bosing:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task BotOnSendMenuAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("BotOnSendMessageAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🛍 Buyurtma berish"), },
            new[] { new KeyboardButton("✍️ Fikr bildirish"),  new KeyboardButton("☎️ Biz bilan aloqa") },
            new[] { new KeyboardButton("ℹ️ Ma'lumot"), new KeyboardButton("⚙️ Sozlamalar")}
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: "Tanlovingizni belgilang:",
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);
    }


    private async Task HandleContactAsync(Message message, CancellationToken cancellationToken)
    {
        var contact = message.Contact;
        var userPhoneNumber = contact.PhoneNumber;
        long chatId = message.Chat.Id;

        var user = new UserCreationDto()
        {
            Phone = userPhoneNumber,
            ChatId = chatId
        };

        await userService.AddAsync(user);

        this.logger.LogInformation("User's phone number: {userPhoneNumber}", userPhoneNumber);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Sizning raqamingiz qabul qilindi. Sizning raqamingiz: {userPhoneNumber}",
            cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Ism sharifingizni kiriting:",
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForUserFullName;
    }

    private async Task HandleUserInfoAsync(Message message, CancellationToken cancellationToken)
    {
        var fullName = message.Text;
        var user = new UserUpdateDto()
        {
            Id = existUser.Id,
            FullName = fullName,
            Phone = existUser.Phone,
            ChatId = message.Chat.Id
        };

        await userService.UpdateAsync(user);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Yaxshi! Buyurtma berishni boshlaymizmi? 😊",
            cancellationToken: cancellationToken);

        await BotOnSendMenuAsync(message, cancellationToken);
        userStates[chatId] = UserState.None;
    }

    private async Task HandlePhoneNumber(Message message, CancellationToken cancellationToken)
    {
        if (Regex.IsMatch(message.Text, @"^\+(\d{12})$"))
        {
            var user = new UserUpdateDto()
            {
                Id = existUser.Id,
                ChatId = message.Chat.Id,
                FullName = existUser.FullName,
                Phone = message.Text
            };

            await userService.UpdateAsync(user);

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Raqamingiz yangilandi. Davom etamizmi?",
            cancellationToken: cancellationToken);

            userStates[chatId] = UserState.None;
            await BotOnSendMenuAsync(message, cancellationToken);
        }
        else
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("🏠 Asosiy menu") }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Faqat telefon raqam kiritilishi kerak.",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
            UpdateType.EditedMessage => HandleEditMessageAsync(botClient, update.EditedMessage, cancellationToken),
            _ => HandleUnknownUpdate(botClient, update, cancellationToken)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            await HandlePollingErrorAsync(botClient, ex, cancellationToken);
        }
    }

    private async Task HandleOrderAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleOrder is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🚖 Yetkazib berish"),  new KeyboardButton("🏃 Olib ketish") },
            new[] { new KeyboardButton("🏠 Asosiy menu") }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    OrderType orderType = new OrderType();
    private async Task HandleDeliveryAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleDeliveryAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("📍 Joriy manzilni yuborish") { RequestLocation = true }
        },
        new[] { new KeyboardButton("🏠 Asosiy menu") }
        })
        {
            ResizeKeyboard = true
        };

        orderType = OrderType.Yetkazib_berish;

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Buyurtmangizni qayerga yetkazib berish kerak 🚙?",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task HandleTakeAwayAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleTakeAwayAsync is working...");

        var branches = await this.filialService.GetAllAsync();

        var additionalButtons = new List<KeyboardButton>
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
            chatId: chatId,
            text: "O'zingizga yaqin filiani tanlang:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForFilialSelection;
    }

    string branch = string.Empty;
    private async Task HandleFilialSelectionAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleFilialSelectionAsync is working...");

        branch = message.Text;

        var existBranch = await this.filialService.GetByLocationAsync(branch);
        if (existBranch is not null)
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Yaxshi! Buyurtma berishni boshlang:",
            cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
        }
        else
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("🏠 Asosiy menu") }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Hozircha bunda filial yo'q",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
        }
    }

    string location = string.Empty;
    private async Task HandleLocationAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleLocationAsync is working..");

        location = message.Location.Latitude.ToString();
        location += message.Location.Longitude.ToString();

        if (location != null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Manzilingiz qabul qilindi!",
                cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Uzr, manzil ma'lumotlari topilmadi.",
                cancellationToken: cancellationToken);
        }
    }

    private async Task HandleInfo(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleInfo is working..");

        await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bu Online-market Telegram boti!",
                cancellationToken: cancellationToken);
    }

    private async Task HandleContactWithUs(Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Biz bilan aloqaga chiqish uchun <<+998905710460>> ga qo'ng'iroq qilishingiz mumkin!",
            cancellationToken: cancellationToken);
    }

    private async Task ShowFeedbackAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleFeedback is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[] { new KeyboardButton("Hammasi yoqdi ♥️")},
        new[] { new KeyboardButton("Yaxshi ⭐️⭐️⭐️⭐️"), new KeyboardButton("Yomon ⭐️") },
        new[] { new KeyboardButton("Yoqmadi ⭐️⭐️"), new KeyboardButton("Juda yomon 👎🏻") },
        new[] { new KeyboardButton("🏠 Asosiy menu") }

        })
        {
            ResizeKeyboard = true
        };


        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Bizni brendimizni tanlagingiz uchun tashakkur!\nFikr bildiring yoki xizmatimizni baholang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task HandleFeedBackAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleFeedBackAsync is working...");

        string userFeedback = message.Text;

        await botClient.SendTextMessageAsync(
            chatId: "@OnlineMarketFeedBack",
            text: $"Yangi Feedback:\n\n{userFeedback}",
            cancellationToken: cancellationToken);

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
           {
                new[] { new KeyboardButton("🏠 Asosiy menu") }
            })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Rahmat! Fikr va mulohazangiz uchun tashakkur!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.None;
    }

    private async Task HandleSettingsAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleSettingsAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Ism sharifni o'zgartirish"),  new KeyboardButton("Raqamni o'zgartirish") },
            new[] { new KeyboardButton("🏠 Asosiy menu") }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Sozlamalar",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task DisplayCategoryKeyboardAsync(long chatId, CancellationToken cancellationToken)
    {
        userStates[chatId] = UserState.WaitingForCategorySelection;

        var categories = await categoryService.GetAllAsync();

        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("🚖 Buyurtma berish"),
        new KeyboardButton("🛒 Savat")
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var category in categories)
        {
            var button = new KeyboardButton(category.Name);
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
            chatId: chatId,
            text: "Mahsulot categoriyasini tanlang:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    IEnumerable<ProductResultDto> products = new List<ProductResultDto>();
    private async Task HandleCategorySelectionAsync(Message message, CancellationToken cancellationToken)
    {
        var selectedCategoryName = message.Text;

        products = await productService.GetByCategoryName(selectedCategoryName);
        if (products.Count() == 0)
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Hozircha bu categoryda mahsulot yo'q",
            cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
        }
        else
        {
            userStates[chatId] = UserState.WaitingForProductSelection;
            await DisplayProductsKeyboardAsync(message.Chat.Id, products, cancellationToken);
        }
    }

    private async Task HandleOrderAction(Message message, CancellationToken cancellationToken)
    {
        var cart = await cartService.GetByUserId(existUser.Id);
        if (cart.Items.Count() > 0)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("Qo'shimcha izoh yo'q")},
            new[] { new KeyboardButton("🏠 Asosiy menu"), new KeyboardButton("⬅️ Categoriya menu") }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Qo'shimcha fikrlaringiz bo'lsa, yozib qoldiring:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

            userStates[chatId] = UserState.WaitingForCommentAction;
        }
        else
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Sizning savatingiz bo'sh. Mahsulot buyurtma bering!",
            cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
        }
    }

    private async Task HandleSaveOrderAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("ShowOrderAsync is working...");

        if (message.Text == "✅ Tasdiqlash")
        {
            var cart = await cartService.GetByUserId(existUser.Id);

            var order = new OrderCreationDto()
            {
                Description = description,
                OrderType = orderType,
                DeliveryAddress = location,
                PaymentMethod = paymentMethod,
                MarketAddress = branch,
                CartId = cart.Id,
            };

            await this.orderService.AddAsync(order);

            if (orderType == OrderType.Yetkazib_berish)
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Salomat bo'ling! Buyurtmangiz tez orada yetkazib beriladi.",
                    cancellationToken: cancellationToken);
            else
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Buyurtma tasdiqlandi. Buyurtmangizni olib ketishingiz mumkin.",
                    cancellationToken: cancellationToken);
        }
        else if (message.Text == "❌ Bekor qilish")
        {
            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);

            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "Yana nimalar sotib olmoqchisiz?",
                   cancellationToken: cancellationToken);
        }
    }

    PaymentMethod paymentMethod = new PaymentMethod();
    private async Task HandlePaymentMethod(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "💵 Naqd pul":
                paymentMethod = PaymentMethod.Naqd;
                break;
            case "💳 Payme":
                paymentMethod = PaymentMethod.Payme;
                break;
            case "💳 Click":
                paymentMethod = PaymentMethod.Click;
                break;
        }

        var cart = await cartService.GetByUserId(existUser.Id);

        var cartItems = await cartItemService.GetByCartId(cart.Id);

        var cartItemsText = string.Join("\n\n", cartItems.Select(item => $"{item.Product.Name}: {item.Quantity} x {item.Price} = {item.Sum}"));
        cartItemsText = "🛒 Savat:\n\n" + cartItemsText;
        cartItemsText = $"Sizning buyurtmangiz:\n\n Buyurtma turi: {orderType.ToString()}\n\n ☎️ Telefon: {existUser.Phone}\n\n 💴 To'lov turi: {paymentMethod.ToString()}\n\n Izohlar: {description}\n\n" + cartItemsText;
        cartItemsText += $"\n\n Jami: {cart.TotalPrice} so'm";

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] {new KeyboardButton("✅ Tasdiqlash") },
            new[] {new KeyboardButton("❌ Bekor qilish") }
        })
        {
            ResizeKeyboard = true
        };

        userStates[chatId] = UserState.WaitingForOrderSaveAction;

        await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: cartItemsText,
                    replyMarkup: replyKeyboard,
                    cancellationToken: cancellationToken);
    }

    string description = string.Empty;
    private async Task HandleDescriptionAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleDescriptionAsync is working...");

        description = message.Text;

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton("💵 Naqd pul") },
            new[] { new KeyboardButton("💳 Payme"), new KeyboardButton("💳 Click") },
            new[] { new KeyboardButton("🏠 Asosiy menu"), new KeyboardButton("⬅️ Product menu") }
            })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
           chatId: chatId,
           text: "Buyurtmangiz uchun to'lov turini tanlang!",
           replyMarkup: replyKeyboard,
           cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForPaymentTypeAction;
    }

    private async Task HandleCartAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCartAsync is working..");

        var cart = await cartService.GetByUserId(existUser.Id);

        var cartItems = await cartItemService.GetByCartId(cart.Id);
        if (cartItems.Count() > 0)
        {
            var cartItemsText = string.Join("\n\n", cartItems.Select(item => $"{item.Product.Name}: {item.Quantity} x {item.Price} = {item.Sum}"));
            cartItemsText = "🛒 Savat:\n\n" + cartItemsText;

            var additionalButtons = new List<KeyboardButton>
            {
            new KeyboardButton("🔄 Tozalash"),
            new KeyboardButton("🚖 Buyurtma berish"),
            new KeyboardButton("🏠 Asosiy menu")
            };

            var allButtons = new List<KeyboardButton[]>();
            var rowButtons = new List<KeyboardButton>();

            foreach (var item in cartItems)
            {
                var button = new KeyboardButton($"❌ {item.Product.Name}");
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

            cartItemsText += $"\n\n Jami: {cart.TotalPrice} so'm";

            await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: $"❌ <Mahsulot nomi> - savatdan o'chirish\n" +
                     $"«🔄 Tozalash» - savatni butunlay bo'shatish",
               cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: cartItemsText,
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            userStates[chatId] = UserState.WaitingForCartAction;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Sizning savatingiz bo'sh. Buyurtma berish uchun mahsulot tanlang",
                cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
        }
    }

    private async Task HandleProductForDeleteAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductForDeleteAsync is working..");

        var cart = await cartService.GetByUserId(existUser.Id);
        var productName = message.Text;

        var isTrue = await cartItemService.DeleteByProductName(cart.Id, productName);
        if (isTrue)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"{productName} savatdan o'chdi!",
                cancellationToken: cancellationToken);

            if (cart.Items.Count == 1)
                await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
            else
                await HandleCartAsync(message, cancellationToken);
        }
    }

    private async Task HandleCleanCartAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCleanCartAsync is working...");

        var cart = await cartService.GetByUserId(existUser.Id);

        var isTrue = await cartItemService.DeleteAllCartItems(cart.Id);
        if (isTrue)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Savat bo'shatildi.",
                cancellationToken: cancellationToken);

            await DisplayCategoryKeyboardAsync(chatId, cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Savatda hech narsa yo'q",
                cancellationToken: cancellationToken);
    }
    private async Task HandleProductSelectionAsync(Message message, CancellationToken cancellationToken)
    {
        var selectedProductName = message.Text;

        var product = await productService.GetByName(selectedProductName);

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"{product.Name}\n {product.Description}\n Mahsulot narxi: {product.Price}",
            cancellationToken: cancellationToken);

        await SendProductInputQuantityAsync(message, cancellationToken);
    }

    string selectedProductName = string.Empty;
    private async Task SendProductInputQuantityAsync(Message message, CancellationToken cancellationToken)
    {
        selectedProductName = message.Text;

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("1"),
            new KeyboardButton("2"),
            new KeyboardButton("3"),
        },
        new[]
        {
            new KeyboardButton("4"),
            new KeyboardButton("5"),
            new KeyboardButton("6"),
        },
        new[]
        {
            new KeyboardButton("7"),
            new KeyboardButton("8"),
            new KeyboardButton("9"),
        },
        new[]
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("⬅️ Product menu"),
        new KeyboardButton("🛒 Savat")
        }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Tanlangan mahsulot miqdorini kiriting:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForQuantityInput;
    }

    private async Task HandleQuantityInputAsync(Message message, CancellationToken cancellationToken)
    {
        int quantity = int.Parse(message.Text);
        if (quantity > 0)
        {
            var product = await productService.GetByName(selectedProductName);
            var user = await userService.GetByChatId(chatId);
            var cart = await cartService.GetByUserId(user.Id);

            var cartItemDto = new CartItemCreationDto
            {
                Quantity = quantity,
                ProductId = product.Id,
                CartId = cart.Id,
                Price = product.Price
            };

            var cartItem = await cartItemService.AddAsync(cartItemDto);

            if (cartItem is not null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{quantity} ta {selectedProductName} savatga qo'shildi!",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Buncha miqdorda mahsulot yo'q!",
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Noto'g'ri miqdor kiritildi. Iltimos, haqiqiy son kiriting.",
                cancellationToken: cancellationToken);
        }
    }

    private async Task DisplayProductsKeyboardAsync(long chatId, IEnumerable<ProductResultDto> products, CancellationToken cancellationToken)
    {
        userStates[chatId] = UserState.WaitingForProductSelection;
        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("⬅️ Categoriya menu"),
        new KeyboardButton("🛒 Savat")
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var product in products)
        {
            var button = new KeyboardButton(product.Name);
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
            chatId: chatId,
            text: "Mahsulot tanlang:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private Task HandleUnknownUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("Update type {update.Type} received", update.Type);

        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogInformation("Error occured with Telegram bot: {ex.Message}", exception);
        return Task.CompletedTask;
    }
}
