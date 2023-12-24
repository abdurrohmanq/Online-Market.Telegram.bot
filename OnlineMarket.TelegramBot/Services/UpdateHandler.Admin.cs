using Telegram.Bot;
using Telegram.Bot.Types;
using OnlineMarket.Domain.Enums;
using OnlineMarket.Service.DTOs.Users;
using OnlineMarket.Service.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;
using OnlineMarket.Domain.Entities.Users;
using OnlineMarket.Service.DTOs.Products;
using OnlineMarket.TelegramBot.Models.Enums;

namespace OnlineMarket.TelegramBot.Services;

public partial class UpdateHandler
{
    private readonly ISecurityService securityService;

    private async Task HandleContactForAdminAsync(Message message, CancellationToken cancellationToken)
    {
        var fullName = message.From.FirstName + " " + message.From.LastName;
        var userName = message.From.Username;
        var phone = message.Contact.PhoneNumber;

        var admin = existAdmin[message.Chat.Id];
        var updateAdmin = new UserUpdateDto()
        {
            Id = admin.Id,
            FullName = fullName,
            ChatId = message.Chat.Id,
            Phone = phone,
            UserName = userName,
            UserRole = Role.Admin,
        };

        await this.userService.UpdateAsync(updateAdmin);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,    
            text: $"{admin.FullName} admin etib tayinlandi!",
            cancellationToken: cancellationToken);

        await DisplayMenuAsync(message, cancellationToken);
    }

    private async Task SendMessageToAdminAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("SendMessageToAdmin is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("Contact ma'lumotlarini jo'natish") {RequestContact = true }
        }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Siz admin emassiz! Admin bo'lish uchun contact ma'lumotlaringizni jo'nating!\nQuyidagi tugmani bosing:",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task<bool> CheckAsync(Message message, CancellationToken cancellationToken)
    {
        var security = await this.securityService.GetAsync();
        if (security is null)
        {
            var create = new Security
            {
                Password = message.Text
            };

            await this.securityService.AddAsync(create);
            return true;
        }
        else
        {
            var check = await this.securityService.CheckAsync(message.Text);
            if(check)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "OK",
                    cancellationToken: cancellationToken);
                return true;
            }
            return false;
        }
    }

    private async Task DisplayMenuAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("DisplayMenuAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Mahsulotlarni boshqarish"),  new KeyboardButton("Filiallarni boshqarish") },
            new[] { new KeyboardButton("Buyurtmalarni boshqarish"),  new KeyboardButton("Parolni o'zgartirish") },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Tanlovni belgilang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForSelectionMenu;
    }

    private async Task HandlerForManageProductAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerForManageProductAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Mahsulot qo'shish"),  new KeyboardButton("Mahsulot o'chirish") },
            new[] { new KeyboardButton("Mahsulotni yangilash"),  new KeyboardButton("Barcha mahsulotlar ro'yxati") },
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

        adminStates[message.Chat.Id] = AdminState.ManageProductPage;
    }

    private async Task DisplayGetAllProductAsync(long chatId, CancellationToken cancellationToken)
    {
        var products = await this.productService.GetAllAsync();

        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("⬅️ Categoriya menu"),
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

        try
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Mahsulot tanlang:",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
    }

    private async Task HandleProductSelectionForAdminAsync(Message message, CancellationToken cancellationToken)
    {
        var selectedProductName = message.Text;

        var product = await productService.GetByName(selectedProductName);
        if (product == null)
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Bunday mahsulot yo'q!",
            cancellationToken: cancellationToken);
        }
        else
        {
            if (isUpdate[message.Chat.Id])
            {
                var productId = product.Id;
                var updatingProduct = new ProductUpdateDto
                {
                    Id = productId,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.Category.Id,
                    Description = product.Description,
                    StockQuantity = product.StockQuantity,
                };

                updateProduct[message.Chat.Id] = updatingProduct;

                var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
            new[] { new KeyboardButton("Mahsulot nomi"),  new KeyboardButton("Mahsulot tavsifi") },
            new[] { new KeyboardButton("Mahsulotni narxi"),  new KeyboardButton("Mahsulor miqdorini") },
            new[] { new KeyboardButton("Mahsulot categoriyasini"),  new KeyboardButton("Barchasini") },
            new[] { new KeyboardButton("⬅️ Ortga") },
            })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"{product.Name}\n{product.Description}\n Mahsulot narxi: {product.Price}\n" +
                    $" Miqdori: {product.StockQuantity}\n Categoriya nomi: {product.Category.Name}\n" +
                    $" Categoriya tavsifi: {product.Category.Description}",
                    cancellationToken: cancellationToken);

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Mahsulotni qaysi qismini o'zgartirmoqchisiz?",
                    replyMarkup: replyKeyboard,
                    cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
            }
            else
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"{product.Name}\n{product.Description}\n Mahsulot narxi: {product.Price}\n" +
                    $" Miqdori: {product.StockQuantity}\n Categoriya nomi: {product.Category.Name}\n" +
                    $" Categoriya tavsifi: {product.Category.Description}",
                    cancellationToken: cancellationToken);
        }
    }

    private Dictionary<long, ProductCreationDto> createProduct = new Dictionary<long, ProductCreationDto>();
    private Dictionary<long, ProductUpdateDto> updateProduct = new Dictionary<long, ProductUpdateDto>();
    private Dictionary<long, bool> isUpdate = new Dictionary<long, bool>();

    private async Task HandleUpdateProductNameAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductNameAsync is working..");

        var productName = message.Text;
        updateProduct[message.Chat.Id].Name = productName;
        if (!allUpdate[message.Chat.Id])
        {
            await this.productService.UpdateAsync(updateProduct[message.Chat.Id]);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Mahsulot nomi"),  new KeyboardButton("Mahsulot tavsifi") },
                new[] { new KeyboardButton("Mahsulotni narxi"),  new KeyboardButton("Mahsulor miqdorini") },
                new[] { new KeyboardButton("Mahsulot categoriyasini"),  new KeyboardButton("Barchasini") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Mahsulotni nomi '{productName}' ga o'zgardi!",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi tavsifini kiriting:",
                            cancellationToken: cancellationToken);
            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductDesc;
        }
    }

    private async Task HandleUpdateProductDescAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductDescAsync is working..");

        var productDesc = message.Text;
        updateProduct[message.Chat.Id].Description = productDesc;
        if (!allUpdate[message.Chat.Id])
        {
            await this.productService.UpdateAsync(updateProduct[message.Chat.Id]);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Mahsulot nomi"),  new KeyboardButton("Mahsulot tavsifi") },
                new[] { new KeyboardButton("Mahsulotni narxi"),  new KeyboardButton("Mahsulor miqdorini") },
                new[] { new KeyboardButton("Mahsulot categoriyasini"),  new KeyboardButton("Barchasini") },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{updateProduct[message.Chat.Id].Name} tavsifi '{productDesc}' ga o'zgardi!",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi narxini kiriting:",
                            cancellationToken: cancellationToken);
            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductPrice;
        }
    }

    private async Task HandleUpdateProductPriceAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductPriceAsync is working..");

        try
        {
            decimal price = decimal.Parse(message.Text);
            updateProduct[message.Chat.Id ].Price = price;
            if (!allUpdate[message.Chat.Id])
            {
                await this.productService.UpdateAsync(updateProduct[message.Chat.Id]);

                var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                new[] { new KeyboardButton("Mahsulot nomi"),  new KeyboardButton("Mahsulot tavsifi") },
                new[] { new KeyboardButton("Mahsulotni narxi"),  new KeyboardButton("Mahsulor miqdorini") },
                new[] { new KeyboardButton("Mahsulot categoriyasini"),  new KeyboardButton("Barchasini") },
                })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{updateProduct[message.Chat.Id].Name} narxi '{price}' ga o'zgardi!",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi miqdorini kiriting:",
                            cancellationToken: cancellationToken);
                adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductQuantity;
            }
        }
        catch 
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Mahsulotni narxi raqamda  kiritilishi kerak!",
            cancellationToken: cancellationToken);
        }
    }

    private async Task HandleUpdateProductQuantityAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductQuantityAsync is working..");

        try
        {
            int quantity = int.Parse(message.Text);
            updateProduct[message.Chat.Id].StockQuantity = quantity;
            if (!allUpdate[message.Chat.Id])
            {
                await this.productService.UpdateAsync(updateProduct[message.Chat.Id]);

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{updateProduct[message.Chat.Id].Name} miqdori '{quantity}' ga o'zgardi!",
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Mahsulotni yangi catgeoriyasini kiriting:",
                            cancellationToken: cancellationToken);

                await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
                adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductCategory;
            }
        }
        catch
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Mahsulotni miqdori raqamda kiritilishi kerak!",
            cancellationToken: cancellationToken);
        }
    }

    private async Task HandleUpdateProductCategoryAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleUpdateProductCategoryAsync is working..");

        var categoryName = message.Text;

        var existCategory = await this.categoryService.GetByName(categoryName);
        if (existCategory != null)
        {
            updateProduct[message.Chat.Id].CategoryId = existCategory.Id;
            await this.productService.UpdateAsync(updateProduct[message.Chat.Id]);

            if (!allUpdate[message.Chat.Id])
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{updateProduct[message.Chat.Id].Name} categoriyasi '{existCategory.Name}' ga o'zgardi!",
                cancellationToken: cancellationToken);
            else
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Mahsulotni barcha qismlari o'zgardi!",
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForUpdateProductProperty;
        }
        else
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Bunday categoriya yo'q!",
            cancellationToken: cancellationToken);
    }

    private async Task HandleCategorySelectionForAdminAsync(Message message, CancellationToken cancellationToken)
    {
        var selectedCategoryName = message.Text;

        products[message.Chat.Id] = await productService.GetByCategoryName(selectedCategoryName);
        if (products.Count() == 0)
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Hozircha bu categoryda mahsulot yo'q",
            cancellationToken: cancellationToken);

            await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
        }
        else
        {
            await DisplayProductsKeyboardForAdminAsync(message.Chat.Id, products[message.Chat.Id], cancellationToken);
        }
    }

    private async Task HandleProductNameAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductNameAsync is working..");

        var productName = message.Text;
        var newProduct = new ProductCreationDto { Name = productName };

        createProduct[message.Chat.Id] = newProduct;

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
            text: "Mahsulot tavsifini kiriting!",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForProductDescription;
    }

    private async Task HandleProductDescAsync(Message  message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductDescAsync is working..");

        var productDesc = message.Text;
        createProduct[message.Chat.Id].Description = productDesc;

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
            text: "Mahsulot narxini kiriting!",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForProductPrice;
    }

    private async Task HandleProductPriceAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductPrice is working..");
        try
        {
            var productPrice = int.Parse(message.Text);
            createProduct[message.Chat.Id].Price = productPrice;

        }
        catch
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Mahsulot narxi raqamda kiritilishi kerakku!!!",
            cancellationToken: cancellationToken);
        }

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
            text: "Mahsulot miqdorini kiriting!",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForProductQuantity;
    }

    private async Task HandleProductQuantityAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductQuantityAsync is working..");
        try
        {
            var productQuantity = int.Parse(message.Text);
            createProduct[message.Chat.Id].StockQuantity = productQuantity;

        }
        catch
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Mahsulot miqdori raqamda kiritilishi kerakku!!!",
            cancellationToken: cancellationToken);
        }

        await DisplayCategoryForAdminAsync(message.Chat.Id, cancellationToken);
        adminStates[message.Chat.Id] = AdminState.WaitingForCategoryId;
    }

    private async Task HandleCategorySelectAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleCategorySelectAsync is working..");

        var existCategory = await this.categoryService.GetByName(message.Text);
        if (existCategory != null)
        {
            createProduct[message.Chat.Id].CategoryId = existCategory.Id;
            await ProductAddAsync(message, cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bunday category yo'q",
                cancellationToken: cancellationToken);
        }
    }

    private async Task ProductAddAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("ProductAddAsync is working..");

        var product = createProduct[message.Chat.Id];

        await this.productService.AddAsync(product);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Mahsulot muvaffaqiyatli qo'shildi!",
            cancellationToken: cancellationToken);

        await HandlerForManageProductAsync(message, cancellationToken);
    }

    private async Task HandleProductSelectAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandleProductSelectAsync is working..");

        var product = await this.productService.GetByName(message.Text.Trim());
        if (product != null)
        {
            await this.productService.DeleteAsync(product.Id);
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{product.Name} o'chirildi.",
                cancellationToken: cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Bunday mahsulot yo'q",
                cancellationToken: cancellationToken);
    }

    private async Task DisplayProductsKeyboardForAdminAsync(long chatId, IEnumerable<ProductResultDto> products, CancellationToken cancellationToken)
    {
        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("⬅️ Categoriya menu"),
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

    private async Task DisplayCategoryForAdminAsync(long chatId, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("DisplayCategoryForAdminAsync is working..");

        var categories = await categoryService.GetAllAsync();

        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton("🏠 Asosiy menu"),
        new KeyboardButton("Yangi yaratish"),
        new KeyboardButton("⬅️ Ortga"),
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

    private async Task HandlerForFilialAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerForFilialAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Yangi filial qo'shish"),  new KeyboardButton("filial o'chirish") },
            new[] { new KeyboardButton("filialni yangilash"),  new KeyboardButton("Barcha filiallar ro'yxati") },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Tanlovingizni belgilang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task HandlerForOrderAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("HandlerForOrderAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Buyurtmani o'chirish"), new KeyboardButton("Ma'lum bir foydalanuvchi buyurtmalari") },
            new[] { new KeyboardButton("Buyurtmani yangilash"),  new KeyboardButton("Barcha buyurtmalar ro'yxati") },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Tanlovingizni belgilang!",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task UpdatePasswordAsync(Message message, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("UpdatePasswordAsync is working..");

        await this.securityService.UpdateAsync(message.Text);

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🏠 Asosiy menu") },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Parol {message.Text} ga o'zgardi",
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }
}
