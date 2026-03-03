using IncidentManagement.API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        if (request.TotalAmount <= 0)
            return BadRequest("Total amount must be greater than zero.");

        if (request.Customer == null)
            return BadRequest("Customer information is required.");

        if (string.IsNullOrWhiteSpace(request.Customer.FullName))
            return BadRequest("Customer name is required.");

        if (string.IsNullOrWhiteSpace(request.Customer.Document))
            return BadRequest("Customer document is required.");

        if (string.IsNullOrWhiteSpace(request.Customer.Email))
            return BadRequest("Customer email is required.");

        var store = await _context.Stores.FindAsync(request.StoreId);
        if (store == null)
            return BadRequest("Store not found.");

        var paymentMethod = await _context.PaymentMethods.FindAsync(request.PaymentMethodId);
        if (paymentMethod == null)
            return BadRequest("Invalid payment method.");

        var channel = await _context.PurchaseChannels.FindAsync(request.ChannelId);
        if (channel == null)
            return BadRequest("Invalid channel.");

        // Regra cartão
        if (paymentMethod.Name == "Credit Card" &&
            (request.Installments == null || request.Installments <= 1))
            return BadRequest("Credit card requires installments greater than 1.");

        if (paymentMethod.Name != "Credit Card")
            request.Installments = null;

        var customer = await _context.Customers
    .FirstOrDefaultAsync(c => c.Document == request.Customer.Document);

        if (customer == null)
        {
            customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = request.Customer.FullName,
                Email = request.Customer.Email,
                Document = request.Customer.Document,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            CustomerId = customer.Id,
            TotalAmount = request.TotalAmount,
            PaymentMethodId = request.PaymentMethodId,
            ChannelId = request.ChannelId,
            StatusId = 1, // Pending
            Installments = request.Installments,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return Ok(order.Id);
    }
}