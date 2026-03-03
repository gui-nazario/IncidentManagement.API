using Microsoft.AspNetCore.Mvc;
using IncidentManagement.API.Infrastructure.Data;
using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/dev")]
public class DevController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public DevController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("generate-customers")]
    public async Task<IActionResult> GenerateCustomers(int quantity = 100)
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var customers = new List<Customer>();

        for (int i = 0; i < quantity; i++)
        {
            customers.Add(new Customer
            {
                Id = Guid.NewGuid(),
                FullName = $"Customer {Guid.NewGuid().ToString().Substring(0, 6)}",
                Email = $"user{Guid.NewGuid().ToString().Substring(0, 6)}@email.com",
                Document = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11),
                CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 365))
            });
        }

        _context.Customers.AddRange(customers);
        await _context.SaveChangesAsync();

        return Ok($"{quantity} customers generated.");
    }
}