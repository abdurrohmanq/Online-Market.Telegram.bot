namespace OnlineMarket.Service.DTOs.Carts;

public class CartItemUpdateDto
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
}
