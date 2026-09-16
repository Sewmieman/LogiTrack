using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogiTrack.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LogiTrack.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly UserManager<LogiTrackUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        UserManager<LogiTrackUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> CreateTokenAsync(LogiTrackUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        claims.AddRange(
            roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT key is missing.");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>(
                "Jwt:ExpirationMinutes"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}