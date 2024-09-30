using Adspro.Server.Domain.Entities;

namespace Adspro.Server.Domain.Interfaces;

public interface IUserSessionRepository
{
    Task AddAsync(UserSession session);
    Task<UserSession?> GetByTokenAsync(string token);
    Task InvalidateAsync(string token);
    Task InvalidateListAsync(IEnumerable<string> sessions);
    Task InvalidateAllAsync(Guid userId);
}