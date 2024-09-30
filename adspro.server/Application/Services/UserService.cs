using Adspro.Server.Application.DTOs;
using Adspro.Server.Application.Interfaces;
using Adspro.Server.Domain.Interfaces;

namespace Adspro.Server.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Sessions = user.UserSessions.Select(session => new UserSessionDto
            {
                Id = session.Id,
                ExpiresAt = session.ExpiresAt
            }).ToList()
        });

        return userDtos;
    }
}
