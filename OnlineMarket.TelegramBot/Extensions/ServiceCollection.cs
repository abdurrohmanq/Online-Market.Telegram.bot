using OnlineMarket.Data.IRepositories;
using OnlineMarket.Data.Repositories;
using OnlineMarket.Service.Interfaces;
using OnlineMarket.Service.Mappers;
using OnlineMarket.Service.Services;

namespace OnlineMarket.TelegramBot.Extensions;

public static class ServiceCollection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFilialService, FilialService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICartItemService, CartItemService>();
    }
}
