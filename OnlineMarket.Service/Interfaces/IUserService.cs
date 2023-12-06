using OnlineMarket.Service.DTOs.Users;

namespace OnlineMarket.Service.Interfaces;

public interface IUserService
{
    Task<UserResultDto> AddAsync(UserCreationDto dto);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<UserResultDto> GetById(long id);
    Task<UserResultDto> GetByChatId(long chatId);
    Task<IEnumerable<UserResultDto>> GetAllAsync();
}
