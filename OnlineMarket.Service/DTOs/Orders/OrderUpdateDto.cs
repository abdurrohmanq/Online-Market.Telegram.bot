using OnlineMarket.Domain.Enums;

namespace OnlineMarket.Service.DTOs.Orders;

public class OrderUpdateDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public OrderType OrderType { get; set; }
    public string DeliveryAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public long CartId { get; set; }
}
