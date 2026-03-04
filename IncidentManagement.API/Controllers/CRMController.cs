using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.API.Infrastructure.Data;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/crm")]
public class CRMController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CRMController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("top-customers")]
    public async Task<IActionResult> TopCustomers(int limit = 10)
    {
        var result = await _context.Orders
            .Where(o => o.StatusId == 2)
            .Include(o => o.Customer)
            .GroupBy(o => new { o.CustomerId, o.Customer.FullName })
            .Select(g => new
            {
                customer = g.Key.FullName,
                revenue = g.Sum(o => o.TotalAmount),
                orders = g.Count(),
                averageTicket = g.Average(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.revenue)
            .Take(limit)
            .ToListAsync();

        return Ok(result);
    }
}