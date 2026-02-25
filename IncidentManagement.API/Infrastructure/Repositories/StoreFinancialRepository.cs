using IncidentManagement.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class StoreFinancialRepository : IStoreFinancialRepository
{
    private readonly ApplicationDbContext _context;

    public StoreFinancialRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<StoreFinancial> financials)
    {
        await _context.StoreFinancials.AddRangeAsync(financials);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> StoreHasFinancialData(Guid storeId)
    {
        return await _context.StoreFinancials
            .AnyAsync(x => x.StoreId == storeId);
    }
    public async Task<List<StoreFinancial>> GetByStoreIdAsync(Guid storeId)
    {
        return await _context.StoreFinancials
            .Where(x => x.StoreId == storeId)
            .ToListAsync();
    }

}