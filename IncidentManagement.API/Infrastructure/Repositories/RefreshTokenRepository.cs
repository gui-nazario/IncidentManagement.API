using IncidentManagement.API.Domain.Entities;
using IncidentManagement.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.API.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }
    public async Task<List<RefreshToken>> GetActiveTokensAsync()
    {
        return await _context.RefreshTokens
            .Where(t => !t.IsRevoked && t.ExpirationDate > DateTime.UtcNow)
            .Include(t => t.User)
            .ToListAsync();
    }
}