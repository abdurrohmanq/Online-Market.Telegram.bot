using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Users;
using OnlineMarket.Service.Helpers;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class SecurityService : ISecurityService
{
    private readonly IRepository<Security> repository;
    public SecurityService(IRepository<Security> repository)
    {
        this.repository = repository;
    }
    public async Task<Security> AddAsync(Security security)
    {
        security.Password = PasswordHash.Hash(security.Password);  
        await this.repository.CreateAsync(security);
        await this.repository.SaveChanges();

        return security;
    }

    public async Task<bool> CheckAsync(string password)
    {
        var existSecurity = await this.repository.GetAsync(s => s.Id == 1);
        if (PasswordHash.Verify(password, existSecurity.Password))
        {
            return true;
        }
        return false;
    }

    public async Task<Security> GetAsync()
    {
        var existSecurity = await this.repository.GetAsync(s => s.Id == 1);

        return existSecurity;
    }

    public async Task<Security> UpdateAsync(string password)
    {
        var existSecurity = await this.repository.GetAsync(s => s.Id == 1);
        existSecurity.Password = PasswordHash.Hash(password);

        this.repository.Update(existSecurity);
        await this.repository.SaveChanges();
        return existSecurity;
    }
}
