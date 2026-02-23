using IncidentManagement.API.Domain.Entities;
using IncidentManagement.API.Infrastructure.Repositories;

namespace IncidentManagement.API.Application.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _repository;

    public StoreService(IStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Store>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Store?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Store> CreateAsync(Store store)
    {
        return await _repository.AddAsync(store);
    }
}