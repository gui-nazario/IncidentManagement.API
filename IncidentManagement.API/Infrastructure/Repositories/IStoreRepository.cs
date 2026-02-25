using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Infrastructure.Repositories;

public interface IStoreRepository
{
    Task<List<Store>> GetAllAsync();
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store> AddAsync(Store store);
    Task<List<Store>> GetInactiveStoresAsync();
    Task<List<Store>> GetStoresAsync(StoreStatus? status);
    Task<IQueryable<Store>> GetQueryableAsync();
}