using Adspro.Server.Domain.Entities;

namespace Adspro.Server.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(string login);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}