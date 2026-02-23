using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IncidentManagement.API.Infrastructure.Repositories;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPut("promote/{username}")]
    public async Task<IActionResult> Promote(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
            return NotFound("Usuário não encontrado.");

        if (user.Role == "Admin")
            return BadRequest("Usuário já é Admin.");

        user.Role = "Admin";

        await _userRepository.UpdateAsync(user);

        return Ok($"Usuário {username} promovido para Admin.");
    }
}