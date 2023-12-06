﻿using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Enums;
using Telegram.Bot.Types;

namespace OnlineMarket.Service.DTOs.Orders;

public class OrderCreationDto
{
    public string Description { get; set; }
    public OrderType OrderType { get; set; }
    public Location DeliveryAddress { get; set; }
    public string MarketAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public long? CartId { get; set; }
}