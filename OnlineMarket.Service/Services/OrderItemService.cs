using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Service.DTOs.Orders;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IMapper mapper;
    private readonly IRepository<Order> orderRepository;
    private readonly IRepository<OrderItem> repository;
    private readonly IRepository<Product> productRepository;
    public OrderItemService(IMapper mapper,
                        IRepository<OrderItem> repository,
                        IRepository<Order> orderRepository,
                        IRepository<Product> productRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
    }

    public async Task<OrderItemResultDto> AddAsync(OrderItemCreationDto dto)
    {
        var existProduct = await productRepository.GetAsync(p => p.Id.Equals(dto.ProductId));
        var existOrder = await this.orderRepository.GetAsync(o => o.Id.Equals(dto.OrderId));
        var mapped = this.mapper.Map<OrderItem>(dto);
        mapped.Product = existProduct;
        mapped.Order = existOrder;

        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<OrderItemResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existOrderItem = await repository.GetAsync(OrderItem => OrderItem.Id.Equals(id))
            ?? throw new NotFoundException($"This OrderItem is not found with id {id}");

        this.repository.Delete(existOrderItem);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<OrderItemResultDto> UpdateAsync(OrderItemUpdateDto dto)
    {
        var existOrderItem = await repository.GetAsync(OrderItem => OrderItem.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This OrderItem is not found with id {dto.Id}");

        this.mapper.Map(dto, existOrderItem);
        this.repository.Update(existOrderItem);
        await this.repository.SaveChanges();

        return this.mapper.Map<OrderItemResultDto>(existOrderItem);
    }

    public async Task<OrderItemResultDto> GetById(long id)
    {
        var existOrderItem = await repository.GetAsync(OrderItem => OrderItem.Id.Equals(id))
            ?? throw new NotFoundException($"This OrderItem is not found with id {id}");

        return this.mapper.Map<OrderItemResultDto>(existOrderItem);
    }

    public async Task<IEnumerable<OrderItemResultDto>> GetAllAsync()
    {
        var allOrderItems = await repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<OrderItemResultDto>>(allOrderItems);
    }
}

