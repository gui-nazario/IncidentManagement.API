using IncidentManagement.API.Application.Common;
using IncidentManagement.API.Application.DTOs;
using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Application.Services;

public interface IStoreService
{
    Task<IEnumerable<Store>> GetAllAsync();
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store> CreateAsync(Store store);
    Task<List<Store>> GetInactiveStoresAsync();
    Task<List<Store>> GetStoresAsync(StoreStatus? status);
    Task<PagedResult<Store>> GetPagedAsync(int page, int pageSize);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(
    string? region,
    string? state,
    StoreStatus? status);
    Task<PagedResult<Store>> GetFilteredAsync(
    string? region,
    string? state,
    StoreStatus? status,
    int page,
    int pageSize);
}