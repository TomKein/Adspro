using Adspro.Server.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adspro.Server.API.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("privateData")]
    public IActionResult GetPrivateData()
    {
        return Ok();
    }
}
