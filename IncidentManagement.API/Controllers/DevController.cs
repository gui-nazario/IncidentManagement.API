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
    [HttpPost("generate-orders")]
    public async Task<IActionResult> GenerateOrders(int quantity = 1000)
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var customers = _context.Customers.ToList();
        var stores = _context.Stores.ToList();
        var paymentMethods = _context.PaymentMethods.ToList();
        var channels = _context.PurchaseChannels.ToList();
        var statuses = _context.OrderStatuses.ToList();

        if (!customers.Any() || !stores.Any())
            return BadRequest("Customers or Stores not found.");

        var orders = new List<Order>();
        var random = new Random();

        for (int i = 0; i < quantity; i++)
        {
            var customer = customers[random.Next(customers.Count)];
            var store = stores[random.Next(stores.Count)];

            // Distribuição inteligente de pagamento
            int paymentRoll = random.Next(1, 101);
            int paymentId = paymentRoll <= 55 ? 1 :
                            paymentRoll <= 85 ? 2 :
                            paymentRoll <= 95 ? 3 : 4;

            // Canal
            int channelRoll = random.Next(1, 101);
            int channelId = channelRoll <= 50 ? 1 :
                            channelRoll <= 80 ? 2 : 3;

            // Status
            int statusRoll = random.Next(1, 101);
            int statusId = statusRoll <= 80 ? 2 :
                           statusRoll <= 90 ? 1 :
                           statusRoll <= 95 ? 3 : 4;

            decimal amount = Math.Round((decimal)(random.NextDouble() * 330 + 20), 2);

            int? installments = null;
            if (paymentId == 2) // Credit Card
                installments = random.Next(1, 7);

            orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                CustomerId = customer.Id,
                TotalAmount = amount,
                PaymentMethodId = paymentId,
                ChannelId = channelId,
                StatusId = statusId,
                Installments = installments,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 120))
            });
        }

        _context.Orders.AddRange(orders);
        await _context.SaveChangesAsync();

        return Ok($"{quantity} orders generated.");
    }

}