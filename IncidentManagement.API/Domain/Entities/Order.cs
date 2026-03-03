using IncidentManagement.API.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public Store Store { get; set; } = null!;

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = null!;

    public int ChannelId { get; set; }
    public PurchaseChannel Channel { get; set; } = null!;

    public int StatusId { get; set; }
    public OrderStatus Status { get; set; } = null!;

    public int? Installments { get; set; }

    public DateTime CreatedAt { get; set; }
}