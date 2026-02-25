using Microsoft.AspNetCore.Authorization;
using IncidentManagement.API.Application.Services;
using IncidentManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly IStoreService _service;

    public StoreController(IStoreService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var store = await _service.GetByIdAsync(id);

        if (store is null)
            return NotFound();

        return Ok(store);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Store store)
    {
        var createdStore = await _service.CreateAsync(store);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdStore.Id },
            createdStore);
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter(
    [FromQuery] string? region,
    [FromQuery] string? state,
    [FromQuery] StoreStatus? status,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetFilteredAsync(region, state, status, page, pageSize);
        return Ok(result);
    }
}