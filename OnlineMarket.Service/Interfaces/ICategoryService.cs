using OnlineMarket.Service.DTOs.Categories;

namespace OnlineMarket.Service.Interfaces;

public interface ICategoryService
{
    Task<CategoryResultDto> AddAsync(CategoryCreationDto dto);
    Task<CategoryResultDto> UpdateAsync(CategoryUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<CategoryResultDto> GetById(long id);
    Task<CategoryResultDto> GetByName(string name);
    Task<IEnumerable<CategoryResultDto>> GetAllAsync();
}
