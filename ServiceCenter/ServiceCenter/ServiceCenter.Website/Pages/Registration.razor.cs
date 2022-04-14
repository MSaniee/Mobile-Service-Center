using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Users;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class Registration
{
    private UserLoginDto _userLoginDto = new();

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    public bool ShowRegistrationErrors { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public async Task Register()
    {
        ShowRegistrationErrors = false;

        var result = await AuthenticationService.RegisterOrLogin(_userLoginDto);

        if (!result.IsSuccess)
        {
            Errors = new List<string> { result.Message };
            ShowRegistrationErrors = true;
        }
        else
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
