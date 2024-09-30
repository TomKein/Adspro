namespace Adspro.Server.Application.DTOs;

public class UserSessionDto
{
    public Guid Id { get; set; }
    public DateTime ExpiresAt { get; set; }
}