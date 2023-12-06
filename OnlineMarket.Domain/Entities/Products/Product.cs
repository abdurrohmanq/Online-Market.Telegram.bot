using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Categories;

namespace OnlineMarket.Domain.Entities.Products;

public class Product : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public long CategoryId { get; set; }
    public Category Category { get; set; }
}
