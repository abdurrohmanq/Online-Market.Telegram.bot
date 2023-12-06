using OnlineMarket.Service.DTOs.Carts;

namespace OnlineMarket.Service.Interfaces;

public interface ICartItemService
{
    Task<CartItemResultDto> AddAsync(CartItemCreationDto dto);
    Task<CartItemResultDto> UpdateAsync(CartItemUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<bool> DeleteByProductName(long cartId, string productName);
    Task<bool> DeleteAllCartItems(long cartId);
    Task<CartItemResultDto> GetByIdAsync(long id);
    Task<IEnumerable<CartItemResultDto>> GetByCartId(long cartId);
    Task<IEnumerable<CartItemResultDto>> GetAllAsync();
}
