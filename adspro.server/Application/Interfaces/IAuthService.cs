using Adspro.Server.API.Models;

namespace Adspro.Server.Application.Interfaces;

public interface IAuthService
{
    Task<UserLoginAnswerModel> LoginAsync(UserLoginModel loginModel);
    Task LogoutAsync(UserLogoutModel logoutModel);
    Task RegisterAsync(UserRegisterModel registerModel);
}