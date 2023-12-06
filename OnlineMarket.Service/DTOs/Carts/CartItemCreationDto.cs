using OnlineMarket.Domain.Entities.Products;

namespace OnlineMarket.Service.DTOs.Carts;

public class CartItemCreationDto
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
}