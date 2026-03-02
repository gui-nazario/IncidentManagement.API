using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IncidentManagement.API.Infrastructure.Repositories;
using IncidentManagement.API.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;

    public UsersController
        (IUserRepository userRepository, ApplicationDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    [HttpPut("{username}/role")]
    public async Task<IActionResult> UpdateRole(string username, [FromBody] UpdateRoleRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
            return NotFound("Usuário não encontrado.");

        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
            return BadRequest("Motivo deve conter pelo menos 10 caracteres.");

        // ✅ guarda role antiga
        var oldRole = user.Role;

        var allowedRoles = new[] { "User", "Admin" };

        var normalizedRole = request.Role.Trim();

        if (!allowedRoles.Any(r =>
            r.Equals(normalizedRole, StringComparison.OrdinalIgnoreCase)))
            return BadRequest("Role inválida.");

        // ✅ altera role
        user.Role = allowedRoles
            .First(r => r.Equals(normalizedRole, StringComparison.OrdinalIgnoreCase));

        await _userRepository.UpdateAsync(user);

        // ✅ cria log DEPOIS da alteração
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = "RoleUpdated",
            PerformedBy = User.Identity!.Name!,
            TargetUser = username,
            OldRole = oldRole,
            NewRole = user.Role,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        return Ok($"Usuário {username} agora é {user.Role}.");
    }
    public record UpdateRoleRequest(
        string Role,
        String Reason
    );
}