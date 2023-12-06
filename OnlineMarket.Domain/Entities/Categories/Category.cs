using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Products;

namespace OnlineMarket.Domain.Entities.Categories;

public class Category : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }
}
