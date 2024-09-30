using Adspro.Server.Domain.Entities;
using Adspro.Server.Domain.Interfaces;
using Adspro.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Adspro.Server.Infrastructure.Repositories;

public class UserRepository(AdsDbContext context) : IUserRepository
{
    public async Task<User?> GetByLoginAsync(string login)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.Include(u => u.UserSessions).ToListAsync();
    }
    
    public async Task AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}