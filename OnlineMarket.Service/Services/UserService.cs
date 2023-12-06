using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Users;
using OnlineMarket.Service.DTOs.Users;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> repository;
    private readonly IRepository<Cart> cartRepository;
    private readonly IMapper mapper;
    public UserService(IRepository<User> repository, IMapper mapper, IRepository<Cart> cartRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.cartRepository = cartRepository;
    }

    public async Task<UserResultDto> AddAsync(UserCreationDto dto)
    {
        var existUser = await repository.GetAsync(user => user.Phone.Equals(dto.Phone));
        if (existUser is not null)
            throw new AlreadyExistException($"This user is already exist with phone {dto.Phone}");

        var mapped = this.mapper.Map<User>(dto);
        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        var cart = new Cart { UserId = mapped.Id, User = mapped };
        await this.cartRepository.CreateAsync(cart);
        await this.cartRepository.SaveChanges();

        return this.mapper.Map<UserResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existUser = await repository.GetAsync(user => user.Id.Equals(id))
            ?? throw new NotFoundException($"This user is not found with id {id}");

        this.repository.Delete(existUser);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {
        var existUser = await repository.GetAsync(user => user.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This user is not found with id {dto.Id}");

        this.mapper.Map(dto, existUser);
        this.repository.Update(existUser);
        await this.repository.SaveChanges();

        return this.mapper.Map<UserResultDto>(existUser);
    }

    public async Task<UserResultDto> GetById(long id)
    {
        var existUser = await repository.GetAsync(user => user.Id.Equals(id))
            ?? throw new NotFoundException($"This user is not found with id {id}");

        return this.mapper.Map<UserResultDto>(existUser);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var allUsers = await repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<UserResultDto>>(allUsers);
    }

    public async Task<UserResultDto> GetByChatId(long chatId)
    {
        var existUser = await repository.GetAsync(user => user.ChatId.Equals(chatId));
        return this.mapper.Map<UserResultDto>(existUser);
    }
}