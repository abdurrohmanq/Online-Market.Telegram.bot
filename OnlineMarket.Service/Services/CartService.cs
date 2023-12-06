using AutoMapper;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Service.DTOs.Carts;
using OnlineMarket.Service.Interfaces;
using OnlineMarket.Domain.Entities.Carts;

namespace OnlineMarket.Service.Services;

public class CartService : ICartService
{
    private readonly IMapper mapper;
    private readonly IRepository<Cart> repository;
    public CartService(IMapper mapper, IRepository<Cart> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }


    public async Task<CartResultDto> GetByUserId(long userId)
    {
        var cart = await this.repository.GetAsync(cart => cart.UserId.Equals(userId), includes: new[] { "Items.Product" });
        return this.mapper.Map<CartResultDto>(cart);
    }
}
