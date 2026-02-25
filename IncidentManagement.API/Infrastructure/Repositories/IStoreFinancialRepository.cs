using IncidentManagement.API.Domain.Entities;

public interface IStoreFinancialRepository
{
    Task AddRangeAsync(List<StoreFinancial> financials);
    Task<bool> StoreHasFinancialData(Guid storeId);
    Task<List<StoreFinancial>> GetByStoreIdAsync(Guid storeId);
}