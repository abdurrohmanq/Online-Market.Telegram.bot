using OnlineMarket.Service.DTOs.Products;

namespace OnlineMarket.Service.DTOs.Orders;

public class OrderItemResultDto
{
    public long Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public OrderResultDto Order { get; set; }
    public ProductResultDto Product { get; set; }
}
