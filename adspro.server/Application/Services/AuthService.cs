using System.Security.Claims;
using Adspro.Server.API.Models;
using Adspro.Server.Application.Interfaces;
using Adspro.Server.Domain.Entities;
using Adspro.Server.Domain.Interfaces;

namespace Adspro.Server.Application.Services;

public class AuthService(
    IUserRepository userRepository, 
    IUserSessionRepository userSessionRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
    : IAuthService
{
    public async Task<UserLoginAnswerModel> LoginAsync(UserLoginModel loginModel)
    {
        var user = await userRepository.GetByLoginAsync(loginModel.Login);
        if (user == null)
            throw new UnauthorizedAccessException("Неверный логин или пароль.");

        var isPasswordValid = passwordHasher.VerifyHashedPassword(user.PasswordHash, loginModel.Password);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Неверный логин или пароль.");

        var expiresAt = DateTime.UtcNow.AddHours(jwtTokenGenerator.GetExpiryHours());
        var token = GenerateJwtToken(user, expiresAt);
        
        var userSession = new UserSession(user.Id, token, expiresAt);
        await userSessionRepository.AddAsync(userSession);

        return new UserLoginAnswerModel() { Token = token, SessionId = userSession.Id.ToString() };
    }
    
    public async Task LogoutAsync(UserLogoutModel logoutModel)
    {
        var user = await userRepository.GetByLoginAsync(logoutModel.Username);
        if (user == null)
            throw new InvalidOperationException("Пользователь не найден.");
        if (logoutModel.Sessions == null)
        {
            await userSessionRepository.InvalidateAllAsync(user.Id);
        }
        else
        {
            await userSessionRepository.InvalidateListAsync(logoutModel.Sessions);
        }
    }
    
    public async Task RegisterAsync(UserRegisterModel registerModel)
    {
        var existingUser = await userRepository.GetByLoginAsync(registerModel.Login);
        if (existingUser != null)
            throw new InvalidOperationException("Пользователь с таким логином уже существует.");

        var hashedPassword = passwordHasher.HashPassword(registerModel.Password);

        var user = new User(registerModel.Login, hashedPassword);

        await userRepository.AddAsync(user);
    }

    private string GenerateJwtToken(User user, DateTime expiresAt)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Expiration, expiresAt.ToString())
        };

        return jwtTokenGenerator.GenerateToken(claims);
    }
}

