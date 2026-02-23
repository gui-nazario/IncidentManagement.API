using Microsoft.AspNetCore.Authorization;
using IncidentManagement.API.Application.Services;
using IncidentManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _service;

    public StoreController(IStoreService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stores = await _service.GetAllAsync();
        return Ok(stores);
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
    public async Task<IActionResult> Create(Store store)
    {
        var createdStore = await _service.CreateAsync(store);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdStore.Id },
            createdStore);
    }
}