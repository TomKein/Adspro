using Adspro.Server.Application.DTOs;

namespace Adspro.Server.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
}