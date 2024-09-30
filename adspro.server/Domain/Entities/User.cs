namespace Adspro.Server.Domain.Entities;

public class User(string login, string passwordHash)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Login { get; init; } = login;
    public string PasswordHash { get; set; } = passwordHash;
    
    public ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}