namespace Adspro.Server.Domain.Entities;

public class UserSession(Guid userId, string jwtToken, DateTime expiresAt)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public string JwtToken { get; set; } = jwtToken;
    public DateTime ExpiresAt { get; set; } = expiresAt;

    public User User { get; set; } = null!;
}