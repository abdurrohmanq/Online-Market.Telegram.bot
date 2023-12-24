namespace OnlineMarket.Service.Helpers;

public static class PasswordHash
{
    public static string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool Verify(string password, string hashPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashPassword);
}
