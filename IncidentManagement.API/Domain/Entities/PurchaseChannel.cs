public class PurchaseChannel
{
    public int Id { get; set; }     // 1 = PhysicalStore
    public string Name { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}