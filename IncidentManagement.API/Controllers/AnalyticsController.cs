using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.API.Infrastructure.Data;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("revenue-by-channel")]
    public async Task<IActionResult> RevenueByChannel()
    {
        var result = await _context.Orders
            .Include(o => o.Channel)
            .Where(o => o.StatusId == 2) // Paid
            .GroupBy(o => o.Channel.Name)
            .Select(g => new
            {
                Channel = g.Key,
                Revenue = g.Sum(o => o.TotalAmount),
                Orders = g.Count()
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("monthly-revenue")]
    public async Task<IActionResult> MonthlyRevenue()
    {
        var result = await _context.Orders
            .Where(o => o.StatusId == 2)
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Revenue = g.Sum(o => o.TotalAmount),
                Orders = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("average-ticket-by-payment")]
    public async Task<IActionResult> AverageTicketByPayment()
    {
        var result = await _context.Orders
            .Include(o => o.PaymentMethod)
            .Where(o => o.StatusId == 2)
            .GroupBy(o => o.PaymentMethod.Name)
            .Select(g => new
            {
                PaymentMethod = g.Key,
                AverageTicket = g.Average(o => o.TotalAmount),
                Orders = g.Count()
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("top-stores")]
    public async Task<IActionResult> TopStores()
    {
        var result = await _context.Orders
            .Include(o => o.Store)
            .Where(o => o.StatusId == 2)
            .GroupBy(o => o.Store.Name)
            .Select(g => new
            {
                Store = g.Key,
                Revenue = g.Sum(o => o.TotalAmount),
                Orders = g.Count()
            })
            .OrderByDescending(x => x.Revenue)
            .Take(10)
            .ToListAsync();

        return Ok(result);
    }
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Orders
            .Where(o => o.StatusId == 2)
            .AsQueryable();
        if (startDate.HasValue)
        {
            var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt >= start);
        }

        if (endDate.HasValue)
        {
            var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt <= end);
        }
        var paidOrders = query;

        var totalRevenue = await paidOrders.SumAsync(o => o.TotalAmount);
        var totalOrders = await paidOrders.CountAsync();
        var averageTicket = totalOrders == 0 ? 0 : totalRevenue / totalOrders;

        var revenueByChannel = await paidOrders
            .Include(o => o.Channel)
            .GroupBy(o => o.Channel.Name)
            .Select(g => new
            {
                channel = g.Key,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count()
            })
            .ToListAsync();

        var topStores = await paidOrders
            .Include(o => o.Store)
            .GroupBy(o => o.Store.Name)
            .Select(g => new
            {
                store = g.Key,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count()
            })
            .OrderByDescending(x => x.revenue)
            .Take(5)
            .ToListAsync();

        var monthlyRevenue = await paidOrders
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new
            {
                year = g.Key.Year,
                month = g.Key.Month,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count()
            })
            .OrderBy(x => x.year)
            .ThenBy(x => x.month)
            .ToListAsync();

        return Ok(new
        {
            totalRevenue,
            totalOrders,
            averageTicket,
            revenueByChannel,
            topStores,
            monthlyRevenue
        });
    }

    [HttpGet("revenue-by-store")]
    public async Task<IActionResult> RevenueByStore(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Orders
            .Where(o => o.StatusId == 2)
            .AsQueryable();

        if (startDate.HasValue)
        {
            var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt >= start);
        }

        if (endDate.HasValue)
        {
            var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt <= end);
        }

        var result = await query
            .GroupBy(o => o.Store.Name)
            .Select(g => new
            {
                store = g.Key,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count()
            })
            .OrderByDescending(x => x.revenue)
            .ToListAsync();

        return Ok(result);
    }
    [HttpGet("orders-by-status")]
    public async Task<IActionResult> OrdersByStatus()
    {
        var result = await _context.Orders
            .GroupBy(o => o.Status.Name)
            .Select(g => new
            {
                status = g.Key,
                orders = g.Count()
            })
            .OrderByDescending(x => x.orders)
            .ToListAsync();

        return Ok(result);
    }
    [HttpGet("payment-method-performance")]
    public async Task<IActionResult> PaymentMethodPerformance(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Orders
            .Where(o => o.StatusId == 2)
            .AsQueryable();

        if (startDate.HasValue)
        {
            var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt >= start);
        }

        if (endDate.HasValue)
        {
            var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt <= end);
        }

        var result = await query
            .GroupBy(o => o.PaymentMethod.Name)
            .Select(g => new
            {
                paymentMethod = g.Key,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count(),
                averageTicket = g.Average(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.revenue)
            .ToListAsync();

        return Ok(result);
    }
    [HttpGet("revenue-growth")]
    public async Task<IActionResult> RevenueGrowth(int? year, int? month)
    {
        var now = DateTime.UtcNow;

        var targetYear = year ?? now.Year;
        var targetMonth = month ?? now.Month;

        if (targetMonth < 1 || targetMonth > 12)
            return BadRequest("Month must be between 1 and 12.");

        var startCurrentMonth = new DateTime(targetYear, targetMonth, 1, 0, 0, 0, DateTimeKind.Utc);
        var startNextMonth = startCurrentMonth.AddMonths(1);
        var startPreviousMonth = startCurrentMonth.AddMonths(-1);

        var currentMonthRevenue = await _context.Orders
            .Where(o => o.StatusId == 2 &&
                        o.CreatedAt >= startCurrentMonth &&
                        o.CreatedAt < startNextMonth)
            .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        var previousMonthRevenue = await _context.Orders
            .Where(o => o.StatusId == 2 &&
                        o.CreatedAt >= startPreviousMonth &&
                        o.CreatedAt < startCurrentMonth)
            .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        decimal growthPercent = 0;

        if (previousMonthRevenue > 0)
        {
            growthPercent = ((currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100;
        }

        return Ok(new
        {
            year = targetYear,
            month = targetMonth,
            currentMonthRevenue,
            previousMonthRevenue,
            growthPercent
        });
    }
    [HttpGet("revenue-trend")]
    public async Task<IActionResult> RevenueTrend(int? year)
    {
        var targetYear = year ?? DateTime.UtcNow.Year;

        var result = await _context.Orders
            .Where(o => o.StatusId == 2 && o.CreatedAt.Year == targetYear)
            .GroupBy(o => o.CreatedAt.Month)
            .Select(g => new
            {
                month = g.Key,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count()
            })
            .OrderBy(x => x.month)
            .ToListAsync();

        return Ok(result);
    }
}