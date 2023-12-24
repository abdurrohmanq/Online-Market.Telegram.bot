using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;
using OnlineMarket.Service.DTOs.Orders;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Data.DbContexts;

namespace OnlineMarket.Service.Services;

public class OrderService : IOrderService
{
    private readonly IMapper mapper;
    private readonly IRepository<Order> repository;
    private readonly IRepository<Cart> cartRepository;
    private readonly ICartItemService cartItemService;
    private readonly IRepository<Product> productRepository;
    public OrderService(IMapper mapper,
                        IRepository<Order> repository,
                        IRepository<Cart> cartRepository,
                        ICartItemService cartItemService,
                        IRepository<Product> productRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.cartRepository = cartRepository;
        this.cartItemService = cartItemService;
        this.productRepository = productRepository;
    }

    public async Task<OrderResultDto> AddAsync(OrderCreationDto dto)
    {
        var existCart = await cartRepository.GetAsync(c => c.Id.Equals(dto.CartId), includes: new[] {"Items", "Items.Product"});
        var mapped = this.mapper.Map<Order>(dto);
        mapped.Cart = existCart;

        foreach(var cartItem in existCart.Items) 
        {
            var product = cartItem.Product;
            product.StockQuantity -= cartItem.Quantity;
            this.productRepository.Update(product);
        }

        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<OrderResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existOrder = await repository.GetAsync(Order => Order.Id.Equals(id))
            ?? throw new NotFoundException($"This Order is not found with id {id}");

        this.repository.Delete(existOrder);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<OrderResultDto> UpdateAsync(OrderUpdateDto dto)
    {
        var existOrder = await repository.GetAsync(Order => Order.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Order is not found with id {dto.Id}");

        this.mapper.Map(dto, existOrder);
        this.repository.Update(existOrder);
        await this.repository.SaveChanges();

        return this.mapper.Map<OrderResultDto>(existOrder);
    }

    public async Task<OrderResultDto> GetById(long id)
    {
        var existOrder = await repository.GetAsync(Order => Order.Id.Equals(id))
            ?? throw new NotFoundException($"This Order is not found with id {id}");

        return this.mapper.Map<OrderResultDto>(existOrder);
    }

    public async Task<IEnumerable<OrderResultDto>> GetAllAsync()
    {
        var allOrders = await repository.GetAll(includes: new[] {"User", "Items"}).ToListAsync();

        return this.mapper.Map<IEnumerable<OrderResultDto>>(allOrders);
    }
}
