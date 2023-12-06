using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Products;

namespace OnlineMarket.Domain.Entities.Carts;

public class CartItem : Auditable
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long? CartId { get; set; }
    public Cart Cart { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }
}
