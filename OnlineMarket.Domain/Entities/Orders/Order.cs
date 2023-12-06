using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Enums;
using Telegram.Bot.Types;

namespace OnlineMarket.Domain.Entities.Orders;

public class Order : Auditable
{
    public string Description { get; set; }
    public OrderType OrderType { get; set; }
    public Location DeliveryAddress { get; set; }
    public string MarketAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public long? CartId { get; set; }
    public Cart Cart { get; set; }
}
