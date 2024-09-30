using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adspro.Server.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Adspro.Server.Infrastructure.Services;

public class JwtTokenGenerator: IJwtTokenGenerator
{
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly double _expiryHours;
    private readonly IConfigurationSection _jwtSettings;
    
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = _jwtSettings["SecretKey"] ?? throw new ArgumentNullException("SecretKey not found in configuration");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        _issuer = _jwtSettings["Issuer"] ?? throw new ArgumentNullException("Issuer not found in configuration");
        _audience = _jwtSettings["Audience"] ?? throw new ArgumentNullException("Audience not found in configuration");
        _expiryHours = double.TryParse(_jwtSettings["ExpiryHours"], out var hours) ? hours : 2;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(_expiryHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            IssuerSigningKey = _key,
            ValidateIssuerSigningKey = true,
        };
    }
    
    public double GetExpiryHours()
    {
        return _expiryHours;
    }
}