using IncidentManagement.API.Domain.Entities;

public class StoreFinancial
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }

    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }

    public Store Store { get; set; } = null!;
}