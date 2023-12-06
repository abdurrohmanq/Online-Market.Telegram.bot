using OnlineMarket.Domain.Enums;

namespace OnlineMarket.Service.DTOs.Users;

public class UserCreationDto
{
    public long ChatId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public Role? UserRole { get; set; }
}
