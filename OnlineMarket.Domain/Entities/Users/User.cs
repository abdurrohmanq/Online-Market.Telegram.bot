using OnlineMarket.Domain.Commons;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Enums;

namespace OnlineMarket.Domain.Entities.Users;

public class User : Auditable
{
    public long ChatId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public Role UserRole { get; set; }

    public ICollection<Order> Orders { get; set; }
}
