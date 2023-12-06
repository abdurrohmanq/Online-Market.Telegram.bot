using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Users;

namespace OnlineMarket.Domain.Entities.Carts;

public class Cart : Auditable
{
    public decimal TotalPrice { get; set; }
    public long? UserId { get; set; }
    public User User { get; set; }

    public ICollection<CartItem> Items { get; set; }
}
