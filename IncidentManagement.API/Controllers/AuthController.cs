using BCrypt.Net;
using IncidentManagement.API.Infrastructure.Repositories;
using IncidentManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IncidentManagement.API.Application.Common;
using Microsoft.AspNetCore.Authorization;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthController(
    IConfiguration configuration,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);

        if (existingUser != null)
            return BadRequest("Usuário já existe.");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        await _userRepository.AddAsync(user);

        return Ok("Usuário criado com sucesso.");
    }

    public record RegisterRequest(string Username, string Password);
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
            return Unauthorized("Usuário inválido.");

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValidPassword)
            return Unauthorized("Senha inválida.");

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        var hashedRefreshToken = PasswordHasher.Hash(refreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            Token = hashedRefreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        // Ainda NÃO estamos salvando no banco

        return Ok(new
        {
            accessToken,
            refreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var activeTokens = await _refreshTokenRepository.GetActiveTokensAsync();

        var storedToken = activeTokens
            .FirstOrDefault(t => PasswordHasher.Verify(request.RefreshToken, t.Token));

        if (storedToken == null)
            return Unauthorized("Refresh token inválido.");

        if (storedToken.IsRevoked)
            return Unauthorized("Refresh token revogado.");

        if (storedToken.ExpirationDate <= DateTime.UtcNow)
            return Unauthorized("Refresh token expirado.");

        var user = storedToken.User;

        if (user == null)
            return Unauthorized();

        // Rotaciona token
        storedToken.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(storedToken);

        var newRefreshToken = GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken
        });
    }
}

public record LoginRequest(string Username, string Password);
public record RefreshRequest(string RefreshToken);