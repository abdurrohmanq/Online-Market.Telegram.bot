using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Filials;
using OnlineMarket.Service.DTOs.Filials;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class FilialService : IFilialService
{
    private readonly IRepository<Filial> repository;
    private readonly IMapper mapper;
    public FilialService(IRepository<Filial> repository, IMapper mapper)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<FilialCustomDto> AddAsync(FilialCreationDto dto)
    {
        var existFilial = await this.repository.GetAsync(c => c.Location.Equals(dto.Location));
        if (existFilial is not null)
            throw new AlreadyExistException("This Filial is already exist!");

        var mapped = this.mapper.Map<Filial>(dto);
        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<FilialCustomDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existFilial = await repository.GetAsync(filial => filial.Id.Equals(id))
            ?? throw new NotFoundException($"This Filial is not found with id {id}");

        this.repository.Delete(existFilial);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<FilialCustomDto> UpdateAsync(FilialCustomDto dto)
    {
        var existFilial = await repository.GetAsync(filial => filial.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Filial is not found with id {dto.Id}");

        this.mapper.Map(dto, existFilial);
        this.repository.Update(existFilial);
        await this.repository.SaveChanges();

        return this.mapper.Map<FilialCustomDto>(existFilial);
    }

    public async Task<FilialCustomDto> GetByIdAsync(long id)
    {
        var existFilial = await repository.GetAsync(filial => filial.Id.Equals(id));

        return this.mapper.Map<FilialCustomDto>(existFilial);
    }

    public async Task<IEnumerable<FilialCustomDto>> GetAllAsync()
    {
        var allCategories = await repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<FilialCustomDto>>(allCategories);
    }

    public async Task<FilialCustomDto> GetByLocationAsync(string location)
    {
        var existFilial = await repository.GetAsync(filial => filial.Location.Equals(location));

        return this.mapper.Map<FilialCustomDto>(existFilial);
    }
}
