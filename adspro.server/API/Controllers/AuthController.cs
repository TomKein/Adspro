using Adspro.Server.API.Models;
using Adspro.Server.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adspro.Server.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginModel loginModel)
    {
        try
        {
            var result = await authService.LoginAsync(loginModel);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Произошла ошибка при обработке вашего запроса. {ex.Message}");
        }
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(UserLogoutModel logoutModel)
    {
        try
        {
            await authService.LogoutAsync(logoutModel);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Произошла ошибка при обработке вашего запроса. {ex.Message}");
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterModel registerModel)
    {
        try
        {
            await authService.RegisterAsync(registerModel);
            return Ok("Пользователь успешно зарегистрирован.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Произошла ошибка при обработке вашего запроса.");
        }
    }
}

