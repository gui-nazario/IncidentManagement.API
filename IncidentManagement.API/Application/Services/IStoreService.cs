using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Application.Services;

public interface IStoreService
{
    Task<IEnumerable<Store>> GetAllAsync();
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store> CreateAsync(Store store);
}