using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Categories;
using OnlineMarket.Service.DTOs.Categories;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> repository;
    private readonly IMapper mapper;
    public CategoryService(IRepository<Category> repository, IMapper mapper)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<CategoryResultDto> AddAsync(CategoryCreationDto dto)
    {
        var existCategory = await this.repository.GetAsync(c => c.Name.Equals(dto.Name));
        if (existCategory is not null)
            throw new AlreadyExistException("This category is already exist!");

        var mapped = this.mapper.Map<Category>(dto);
        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<CategoryResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existCategory = await repository.GetAsync(Category => Category.Id.Equals(id))
            ?? throw new NotFoundException($"This Category is not found with id {id}");

        this.repository.Delete(existCategory);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<CategoryResultDto> UpdateAsync(CategoryUpdateDto dto)
    {
        var existCategory = await repository.GetAsync(Category => Category.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Category is not found with id {dto.Id}");

        this.mapper.Map(dto, existCategory);
        this.repository.Update(existCategory);
        await this.repository.SaveChanges();

        return this.mapper.Map<CategoryResultDto>(existCategory);
    }

    public async Task<CategoryResultDto> GetById(long id)
    {
        var existCategory = await repository.GetAsync(Category => Category.Id.Equals(id))
            ?? throw new NotFoundException($"This Category is not found with id {id}");

        return this.mapper.Map<CategoryResultDto>(existCategory);
    }

    public async Task<IEnumerable<CategoryResultDto>> GetAllAsync()
    {
        var allCategories = await repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<CategoryResultDto>>(allCategories);
    }

    public async Task<CategoryResultDto> GetByName(string name)
    {
        var existCategory = await repository.GetAsync(c => c.Name.Equals(name));
        return this.mapper.Map<CategoryResultDto>(existCategory);
    }
}
