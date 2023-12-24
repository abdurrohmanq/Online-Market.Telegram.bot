using OnlineMarket.Domain.Enums;
using OnlineMarket.Service.DTOs.Carts;
using OnlineMarket.Service.DTOs.Users;

namespace OnlineMarket.Service.DTOs.Orders;

public class OrderResultDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public OrderType OrderType { get; set; }
    public string DeliveryAddress { get; set; }
    public string MarketAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public CartResultDto Cart { get; set; }
    public UserResultDto User { get; set; }
    public ICollection<OrderItemResultDto> Items { get; set; }

}
