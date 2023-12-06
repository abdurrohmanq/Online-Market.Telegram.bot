using OnlineMarket.Service.DTOs.Products;

namespace OnlineMarket.Service.Interfaces;

public interface IProductService
{
    Task<ProductResultDto> AddAsync(ProductCreationDto dto);
    Task<ProductResultDto> UpdateAsync(ProductUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<ProductResultDto> GetById(long id);
    Task<ProductResultDto> GetByName(string name);
    Task<IEnumerable<ProductResultDto>> GetByCategoryName(string name);
    Task<IEnumerable<ProductResultDto>> GetAllAsync();
}
