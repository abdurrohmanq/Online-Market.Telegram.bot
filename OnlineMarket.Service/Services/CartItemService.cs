using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Service.DTOs.Carts;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class CartItemService : ICartItemService
{
    private readonly IRepository<CartItem> repository;
    private readonly IRepository<Cart> cartRepository;
    private readonly IRepository<Product> productRepository;
    private readonly IMapper mapper;
    public CartItemService(IRepository<CartItem> repository, IMapper mapper, IRepository<Product> productRepository, IRepository<Cart> cartRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.cartRepository = cartRepository;
        this.productRepository = productRepository;
    }
    public async Task<CartItemResultDto> AddAsync(CartItemCreationDto dto)
    {
        var existProduct = await this.productRepository.GetAsync(p => p.Id.Equals(dto.ProductId));
        var existCart = await this.cartRepository.GetAsync(c => c.Id.Equals(dto.CartId));

        if (dto.Quantity > existProduct.StockQuantity)
            return null;

        var mapped = this.mapper.Map<CartItem>(dto);
        mapped.Sum = mapped.Quantity * mapped.Price;
        existCart.TotalPrice += mapped.Sum;
        mapped.Product = existProduct;
        mapped.Cart = existCart;

        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<CartItemResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existCartItem = await repository.GetAsync(CartItem => CartItem.Id.Equals(id))
            ?? throw new NotFoundException($"This CartItem is not found with id {id}");

        this.repository.Delete(existCartItem);
        await this.repository.SaveChanges();
        return true;
    }

    public async Task<CartItemResultDto> UpdateAsync(CartItemUpdateDto dto)
    {
        var existCartItem = await repository.GetAsync(CartItem => CartItem.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This CartItem is not found with id {dto.Id}");

        this.mapper.Map(dto, existCartItem);
        this.repository.Update(existCartItem);
        await this.repository.SaveChanges();

        return this.mapper.Map<CartItemResultDto>(existCartItem);
    }

    public async Task<CartItemResultDto> GetByIdAsync(long id)
    {
        var existCartItem = await repository.GetAsync(CartItem => CartItem.Id.Equals(id))
            ?? throw new NotFoundException($"This CartItem is not found with id {id}");

        return this.mapper.Map<CartItemResultDto>(existCartItem);
    }

    public async Task<IEnumerable<CartItemResultDto>> GetAllAsync()
    {
        var allCartItems = await repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<CartItemResultDto>>(allCartItems);
    }

    public async Task<IEnumerable<CartItemResultDto>> GetByCartId(long cartId)
    {
        var cartItems = await repository.GetAll(c => c.CartId.Equals(cartId), includes: new[] {"Product", "Cart"}).ToListAsync();
        return this.mapper.Map<IEnumerable<CartItemResultDto>>(cartItems);
    }

    public async Task<bool> DeleteByProductName(long cartId, string productName)
    {
        productName = productName.Trim('❌',' ');
        var existCartItems = await GetByCartId(cartId);
        var existCart = await this.cartRepository.GetAsync(c => c.Id.Equals(cartId), includes: new[] {"Items"});
        var existCartItem = existCartItems.FirstOrDefault(item => item.Product.Name.Equals(productName));
        var cartItem = await repository.GetAsync(c => c.Id.Equals(existCartItem.Id));
        if (existCartItem != null)
        {
            this.repository.Destroy(cartItem);
            existCart.TotalPrice -= cartItem.Sum;
            this.cartRepository.Update(existCart);
            await this.cartRepository.SaveChanges();
            return true;
        }
        else
            return false;
    }

    public async Task<bool> DeleteAllCartItems(long cartId)
    {
        var existCart = await this.cartRepository.GetAsync(c => c.Id.Equals(cartId));
        existCart.TotalPrice = 0;
        this.repository.Destroy(cart => cart.CartId.Equals(cartId));
        this.cartRepository.Update(existCart);
        await this.repository.SaveChanges();

        return true;
    }
}
