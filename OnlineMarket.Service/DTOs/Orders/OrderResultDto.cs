using OnlineMarket.Domain.Enums;
using OnlineMarket.Service.DTOs.Carts;
using Telegram.Bot.Types;

namespace OnlineMarket.Service.DTOs.Orders;

public class OrderResultDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public OrderType OrderType { get; set; }
    public Location DeliveryAddress { get; set; }
    public string MarketAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public CartResultDto Cart { get; set; }
}
