using OnlineMarket.Service.DTOs.Orders;

namespace OnlineMarket.Service.Interfaces;

public interface IOrderItemService
{
    Task<OrderItemResultDto> AddAsync(OrderItemCreationDto dto);
    Task<OrderItemResultDto> UpdateAsync(OrderItemUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<OrderItemResultDto> GetById(long id);
    Task<IEnumerable<OrderItemResultDto>> GetAllAsync();
}
