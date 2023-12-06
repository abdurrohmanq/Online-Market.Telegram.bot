using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Service.DTOs.Users;

namespace OnlineMarket.Service.DTOs.Carts;

public class CartResultDto
{
    public long Id { get; set; }
    public decimal TotalPrice { get; set; }
    public UserResultDto User { get; set; }
    public ICollection<CartItemResultDto> Items { get; set; }
}
