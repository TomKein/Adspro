using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Adspro.Server.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(IEnumerable<Claim> claims);
    TokenValidationParameters GetValidationParameters();
    double GetExpiryHours();
}