using OnlineMarket.Service.DTOs.Categories;

namespace OnlineMarket.Service.DTOs.Products;

public class ProductResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public CategoryResultDto Category { get; set; }
}
