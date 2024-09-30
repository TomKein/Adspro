namespace Adspro.Server.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public List<UserSessionDto> Sessions { get; set; } = new();
}