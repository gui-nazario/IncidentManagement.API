public class CreateOrderRequest
{
    public Guid StoreId { get; set; }
    public CustomerDto Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public int PaymentMethodId { get; set; }
    public int ChannelId { get; set; }
    public int? Installments { get; set; }
}

public class CustomerDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Document { get; set; } = null!;
}