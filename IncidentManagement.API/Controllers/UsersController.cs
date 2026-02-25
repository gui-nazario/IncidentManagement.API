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

    [HttpPut("{username}/role")]
    public async Task<IActionResult> UpdateRole(string username, [FromBody] UpdateRoleRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
            return NotFound("Usuário não encontrado.");

        var allowedRoles = new[] { "User", "Admin" };

        var normalizedRole = request.Role.Trim();

        if (!allowedRoles.Any(r => r.Equals(normalizedRole, StringComparison.OrdinalIgnoreCase)))
            return BadRequest("Role inválida.");

        user.Role = allowedRoles
            .First(r => r.Equals(normalizedRole, StringComparison.OrdinalIgnoreCase));

        user.Role = request.Role;

        await _userRepository.UpdateAsync(user);

        return Ok($"Usuário {username} agora é {request.Role}.");
    }
    public record UpdateRoleRequest(string Role);
}