using IncidentManagement.API.Application.Services;
using IncidentManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IStoreService _storeService;

    public DashboardController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
    [FromQuery] string? region,
    [FromQuery] string? state,
    [FromQuery] StoreStatus? status)
    {
        var result = await _storeService.GetDashboardSummaryAsync(region, state, status);
        return Ok(result);
    }
}