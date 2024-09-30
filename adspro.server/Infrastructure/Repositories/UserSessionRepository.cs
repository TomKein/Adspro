using Adspro.Server.Domain.Entities;
using Adspro.Server.Domain.Interfaces;
using Adspro.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Adspro.Server.Infrastructure.Repositories;

public class UserSessionRepository(AdsDbContext context) : IUserSessionRepository
{
    public async Task AddAsync(UserSession session)
    {
        context.UserSessions.Add(session);
        await context.SaveChangesAsync();
    }

    public async Task<UserSession?> GetByTokenAsync(string token)
    {
        return await context.UserSessions
            .FirstOrDefaultAsync(s => s.JwtToken == token);
    }

    public async Task InvalidateAsync(string token)
    {
        var session = await GetByTokenAsync(token);
        if (session != null)
        {
            context.UserSessions.Remove(session);
            await context.SaveChangesAsync();
        }
    }

    public async Task InvalidateListAsync(IEnumerable<string> tokens)
    {
        var sessions = context.UserSessions.Where(s => tokens.Contains(s.Id.ToString()));
        await RemoveSessionsAsync(sessions);
    }

    public async Task InvalidateAllAsync(Guid userId)
    {
        var sessions = context.UserSessions.Where(s => s.UserId == userId);
        await RemoveSessionsAsync(sessions);
    }

    private async Task RemoveSessionsAsync(IQueryable<UserSession> sessions)
    {
        if (!sessions.Any()) return;
        context.UserSessions.RemoveRange(sessions);
        await context.SaveChangesAsync();
    }
}