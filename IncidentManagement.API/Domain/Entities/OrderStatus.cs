public class OrderStatus
{
    public int Id { get; set; }     // 1 = Pending
    public string Name { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}