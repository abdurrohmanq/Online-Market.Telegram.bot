using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Service.DTOs.Products;

namespace OnlineMarket.Service.DTOs.Carts;

public class CartItemResultDto
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public Cart Cart { get; set; }
    public ProductResultDto Product { get; set; }
}
