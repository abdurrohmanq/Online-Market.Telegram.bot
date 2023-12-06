using Telegram.Bot;
using Telegram.Bot.Polling;
using OnlineMarket.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.TelegramBot.Services;
using OnlineMarket.TelegramBot.Extensions;
using AspectCore.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var token = builder.Configuration.GetValue("Token", string.Empty);

builder.Services.AddServices();

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token));
builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();

builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.Run();
