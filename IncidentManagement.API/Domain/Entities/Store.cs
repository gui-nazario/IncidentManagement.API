namespace IncidentManagement.API.Domain.Entities;

public class Store
{
    public Guid Id { get; set; }

    // Identificação
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;

    // Localização
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Status operacional
    public StoreStatus Status { get; set; } = StoreStatus.Active;

    // Indicadores financeiros
    public decimal MonthlyRevenue { get; set; }
    public decimal MonthlyExpenses { get; set; }

    // Indicadores operacionais
    public int EmployeesCount { get; set; }
    public int DailyCustomersAverage { get; set; }

    // Datas estratégicas
    public DateTime OpenedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<StoreFinancial> Financials { get; set; } = new List<StoreFinancial>();
}