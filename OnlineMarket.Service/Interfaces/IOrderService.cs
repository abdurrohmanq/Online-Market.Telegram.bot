using OnlineMarket.Service.DTOs.Orders;

namespace OnlineMarket.Service.Interfaces;

public interface IOrderService
{
    Task<OrderResultDto> AddAsync(OrderCreationDto dto);
    Task<OrderResultDto> UpdateAsync(OrderUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<OrderResultDto> GetById(long id);
    Task<IEnumerable<OrderResultDto>> GetByUserAsync(string query);
    Task<IEnumerable<OrderResultDto>> GetAllAsync();
}
