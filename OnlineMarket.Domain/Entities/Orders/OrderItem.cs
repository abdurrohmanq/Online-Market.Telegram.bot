using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Products;

namespace OnlineMarket.Domain.Entities.Orders;

public class OrderItem : Auditable
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }
}
