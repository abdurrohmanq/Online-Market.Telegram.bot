using OnlineMarket.Domain.Entities.Users;

namespace OnlineMarket.Service.Interfaces;

public interface ISecurityService
{
    Task<Security> AddAsync(Security security);
    Task<Security> GetAsync();
    Task<bool> CheckAsync(string password);
    Task<Security> UpdateAsync(string password);
}
