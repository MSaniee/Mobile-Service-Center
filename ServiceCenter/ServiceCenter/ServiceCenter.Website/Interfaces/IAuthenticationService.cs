using ServiceCenter.Application.Dtos.Users;
using ServiceCenter.Application.Services.Base;

namespace ServiceCenter.Website.Interfaces;

public interface IAuthenticationService
{
    Task<SResult> SendCode(string phoneNumber);
    Task<SResult> RegisterOrLogin(UserLoginDto dto);
    //Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
    Task Logout();
}
