using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Entities.Categories;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Service.DTOs.Products;
using OnlineMarket.Service.Exceptions;
using OnlineMarket.Service.Interfaces;

namespace OnlineMarket.Service.Services;

public class ProductService : IProductService
{
    private readonly IMapper mapper;
    private readonly IRepository<Product> repository;
    private readonly IRepository<Category> categoryRepository;
    public ProductService(IMapper mapper,
                          IRepository<Product> repository,
                          IRepository<Category> categoryRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.categoryRepository = categoryRepository;
    }

    public async Task<ProductResultDto> AddAsync(ProductCreationDto dto)
    {
        var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
            ?? throw new NotFoundException("This category is not found!");

        var mapped = this.mapper.Map<Product>(dto);
        await this.repository.CreateAsync(mapped);
        await this.repository.SaveChanges();

        return this.mapper.Map<ProductResultDto>(mapped);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existProduct = await repository.GetAsync(product => product.Id.Equals(id))
            ?? throw new NotFoundException($"This Product is not found with id {id}");

        this.repository.Delete(existProduct);
        await this.repository.SaveChanges();

        return true;
    }

    public async Task<ProductResultDto> UpdateAsync(ProductUpdateDto dto)
    {
        var existProduct = await repository.GetAsync(product => product.Id.Equals(dto.Id), includes: new[] {"Category"})
            ?? throw new NotFoundException($"This Product is not found with id {dto.Id}");
        if(existProduct.CategoryId != dto.CategoryId)
        {
            var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
                ?? throw new NotFoundException("This category is not found!");

            existProduct.Category = existCategory;
            this.categoryRepository.Update(existCategory);
        }

        this.mapper.Map(dto, existProduct);
        this.repository.Update(existProduct);
        await this.repository.SaveChanges();

        return this.mapper.Map<ProductResultDto>(existProduct);
    }

    public async Task<ProductResultDto> GetByIdAsync(long id)
    {
        var existProduct = await repository.GetAsync(product => product.Id.Equals(id))
            ?? throw new NotFoundException($"This Product is not found with id {id}");

        return this.mapper.Map<ProductResultDto>(existProduct);
    }

    public async Task<IEnumerable<ProductResultDto>> GetAllAsync()
    {
        var allProducts = await repository.GetAll(includes: new[] {"Category"}).ToListAsync();

        return this.mapper.Map<IEnumerable<ProductResultDto>>(allProducts);
    }

    public async Task<IEnumerable<ProductResultDto>> GetByCategoryName(string name)
    {
        var existCategory = await categoryRepository.GetAsync(c => c.Name.Equals(name))
            ?? throw new NotFoundException("This category is not found");

        var products = await repository.GetAll(c => c.CategoryId.Equals(existCategory.Id)).ToListAsync();

        return this.mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<ProductResultDto> GetByName(string name)
    {
        var existProduct = await repository.GetAsync(p => p.Name.Equals(name), includes: new[] { "Category" });
        return this.mapper.Map<ProductResultDto>(existProduct);
    }
}

