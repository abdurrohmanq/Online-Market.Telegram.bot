namespace OnlineMarket.Service.DTOs.Orders;

public class OrderItemCreationDto
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}