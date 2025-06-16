using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shiftly.Models;
using Shiftly.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _repo;
    private readonly IConfiguration _config;

    public AuthController(UserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _repo.GetByUserNameAsync(dto.UserName);
        if (user is null) return Unauthorized("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid password");

        var token = GenerateJwt(user);
        return Ok(new { token });
    }

    private string GenerateJwt(User user)
    {
        var jwtKey = _config["Jwt:Key"];
        var jwtIssuer = _config["Jwt:Issuer"];
        var jwtAudience = _config["Jwt:Audience"];

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role ?? "User"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}