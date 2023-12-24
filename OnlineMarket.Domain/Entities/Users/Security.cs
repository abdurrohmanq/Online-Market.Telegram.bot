using OnlineMarket.Domain.Commons;

namespace OnlineMarket.Domain.Entities.Users;

public class Security : Auditable
{
    public string Password { get; set; }
}
