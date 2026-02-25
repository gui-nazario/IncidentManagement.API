using Microsoft.AspNetCore.Mvc;
using IncidentManagement.API.Infrastructure.Repositories;
using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialController : ControllerBase
{
    private readonly IStoreRepository _storeRepository;
    private readonly IStoreFinancialRepository _financialRepository;

    public FinancialController(
        IStoreRepository storeRepository,
        IStoreFinancialRepository financialRepository)
    {
        _storeRepository = storeRepository;
        _financialRepository = financialRepository;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        var stores = await _storeRepository.GetAllAsync();

        if (stores == null || !stores.Any())
            return BadRequest("Nenhuma loja encontrada.");

        var random = new Random();

        foreach (var store in stores)
        {
            var hasData = await _financialRepository.StoreHasFinancialData(store.Id);

            if (hasData)
                continue;

            var financials = new List<StoreFinancial>();

            decimal baseRevenue = random.Next(50000, 150000);

            for (int i = 11; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddMonths(-i);

                var growthFactor = 1 + (decimal)(random.NextDouble() * 0.05);
                baseRevenue *= growthFactor;

                var expenses = baseRevenue * (decimal)(0.6 + random.NextDouble() * 0.25);

                financials.Add(new StoreFinancial
                {
                    Id = Guid.NewGuid(),
                    StoreId = store.Id,
                    Year = date.Year,
                    Month = date.Month,
                    Revenue = Math.Round(baseRevenue, 2),
                    Expenses = Math.Round(expenses, 2)
                });
            }

            await _financialRepository.AddRangeAsync(financials);
        }

        return Ok("Seed financeiro gerado com sucesso.");
    }
    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetStoreFinancial(Guid storeId)
    {
        var data = await _financialRepository
            .GetByStoreIdAsync(storeId);

        if (!data.Any())
            return NotFound("Nenhum dado financeiro encontrado.");

        var ordered = data
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .Select(x => new
            {
                x.Year,
                x.Month,
                x.Revenue,
                x.Expenses,
                Profit = x.Revenue - x.Expenses
            });

        return Ok(ordered);
    }
    [HttpGet("store/{storeId}/growth")]
    public async Task<IActionResult> GetStoreGrowth(Guid storeId)
    {
        var data = await _financialRepository.GetByStoreIdAsync(storeId);

        if (data == null || !data.Any())
            return NotFound("Nenhum dado financeiro encontrado.");

        var ordered = data
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        var first = ordered.First();
        var last = ordered.Last();

        if (first.Revenue == 0)
            return BadRequest("Não é possível calcular crescimento com receita inicial zero.");

        var growth = ((last.Revenue - first.Revenue) / first.Revenue) * 100;

        return Ok(new
        {
            firstMonthRevenue = first.Revenue,
            lastMonthRevenue = last.Revenue,
            growthPercentage = Math.Round(growth, 2)
        });
    }
    [HttpGet("store/{storeId}/metrics")]
    public async Task<IActionResult> GetStoreMetrics(Guid storeId)
    {
        var data = await _financialRepository.GetByStoreIdAsync(storeId);

        if (data == null || data.Count < 2)
            return BadRequest("Dados insuficientes para cálculo.");

        var ordered = data
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        var first = ordered.First();
        var last = ordered.Last();

        var totalGrowth = ((last.Revenue - first.Revenue) / first.Revenue) * 100;

        var lastMonth = ordered[^1];
        var previousMonth = ordered[^2];

        var lastMonthGrowth = ((lastMonth.Revenue - previousMonth.Revenue) / previousMonth.Revenue) * 100;

        var annualRevenue = ordered.Sum(x => x.Revenue);

        var yearsSpan = (ordered.Count / 12.0);
        var cagr = (Math.Pow((double)(last.Revenue / first.Revenue), 1 / yearsSpan) - 1) * 100;

        return Ok(new
        {
            totalGrowth = Math.Round(totalGrowth, 2),
            lastMonthGrowth = Math.Round(lastMonthGrowth, 2),
            annualRevenue = Math.Round(annualRevenue, 2),
            cagr = Math.Round(cagr, 2)
        });
    }
}