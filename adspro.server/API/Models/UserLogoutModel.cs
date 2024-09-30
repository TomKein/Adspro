namespace Adspro.Server.API.Models;

public class UserLogoutModel
{
    public string Username { get; set; } = null!;
    public IEnumerable<string>? Sessions { get; set; } = null;
}