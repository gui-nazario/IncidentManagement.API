public class PaymentMethod
{
    public int Id { get; set; }     // 1 = Pix
    public string Name { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
   }