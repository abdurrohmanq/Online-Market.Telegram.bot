using OnlineMarket.Service.DTOs.Filials;

namespace OnlineMarket.Service.Interfaces;

public interface IFilialService
{
    Task<FilialCustomDto> AddAsync(FilialCreationDto dto);
    Task<FilialCustomDto> UpdateAsync(FilialCustomDto dto);
    Task<bool> DeleteAsync(long id);
    Task<FilialCustomDto> GetByIdAsync(long id);
    Task<FilialCustomDto> GetByLocationAsync(string location);
    Task<IEnumerable<FilialCustomDto>> GetAllAsync();
}
