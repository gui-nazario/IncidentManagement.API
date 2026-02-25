using IncidentManagement.API.Domain.Entities;
using IncidentManagement.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.API.Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly ApplicationDbContext _context;

    public StoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Store> AddAsync(Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<List<Store>> GetAllAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<Store?> GetByIdAsync(Guid id)
    {
        return await _context.Stores
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<Store>> GetInactiveStoresAsync()
    {
        return await _context.Stores
            .Where(s => s.Status == StoreStatus.Inactive)
            .ToListAsync();
    }

    public async Task<List<Store>> GetStoresAsync(StoreStatus? status)
    {
        var query = _context.Stores.AsQueryable();

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        return await query.ToListAsync();
    }
    public Task<IQueryable<Store>> GetQueryableAsync()
    {
        return Task.FromResult(_context.Stores.AsQueryable());
    }
}