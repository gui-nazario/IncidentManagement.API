using IncidentManagement.API.Application.Common;
using IncidentManagement.API.Domain.Entities;
using IncidentManagement.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.API.Application.DTOs;

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
    public async Task<List<Store>> GetInactiveStoresAsync()
    {
        return await _repository.GetInactiveStoresAsync();
    }
    public async Task<List<Store>> GetStoresAsync(StoreStatus? status)
    {
        return await _repository.GetStoresAsync(status);
    }
    public async Task<PagedResult<Store>> GetPagedAsync(int page, int pageSize)
    {
        var query = await _repository.GetQueryableAsync();

        var totalItems = await query.CountAsync();

        var data = await query
            .OrderBy(s => s.Name) // importante para paginação consistente
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Store>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(
    string? region,
    string? state,
    StoreStatus? status)
    {
        var query = await _repository.GetQueryableAsync();

        if (!string.IsNullOrEmpty(region))
            query = query.Where(s => s.Region == region);

        if (!string.IsNullOrEmpty(state))
            query = query.Where(s => s.State == state);

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        var totalStores = await query.CountAsync();

        var activeStores = await query.CountAsync(s => s.Status == StoreStatus.Active);
        var inactiveStores = await query.CountAsync(s => s.Status == StoreStatus.Inactive);

        var totalRevenue = await query.SumAsync(s => s.MonthlyRevenue);
        var totalExpenses = await query.SumAsync(s => s.MonthlyExpenses);

        var averageCustomers =
            await query.AverageAsync(s => (double?)s.DailyCustomersAverage) ?? 0;

        return new DashboardSummaryDto
        {
            TotalStores = totalStores,
            ActiveStores = activeStores,
            InactiveStores = inactiveStores,
            TotalRevenue = totalRevenue,
            TotalExpenses = totalExpenses,
            TotalProfit = totalRevenue - totalExpenses,
            AverageDailyCustomers = averageCustomers
        };
    }
    public async Task<PagedResult<Store>> GetFilteredAsync(
    string? region,
    string? state,
    StoreStatus? status,
    int page,
    int pageSize)
    {
        var query = await _repository.GetQueryableAsync();

        if (!string.IsNullOrEmpty(region))
            query = query.Where(s => s.Region == region);

        if (!string.IsNullOrEmpty(state))
            query = query.Where(s => s.State == state);

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        var totalItems = await query.CountAsync();

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Store>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }
}