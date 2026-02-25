namespace IncidentManagement.API.Application.DTOs;

public class DashboardSummaryDto
{
    public int TotalStores { get; set; }
    public int ActiveStores { get; set; }
    public int InactiveStores { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalProfit { get; set; }
    public double AverageDailyCustomers { get; set; }
}