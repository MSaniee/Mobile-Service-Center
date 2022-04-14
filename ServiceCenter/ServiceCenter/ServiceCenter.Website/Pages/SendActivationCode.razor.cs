using Microsoft.AspNetCore.Components;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class SendActivationCode
{
    private string _phoneNumber = "";

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    public bool ShowRegistrationErrors { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public async Task Register()
    {
        ShowRegistrationErrors = false;
        var sendCodeResult = await AuthenticationService.SendCode(_phoneNumber);

        if (!sendCodeResult.IsSuccess)
        {
            Errors = new List<string> { sendCodeResult.Message };
            ShowRegistrationErrors = true;
        }
        else
        {
            NavigationManager.NavigateTo("/registrOrLogin");
        }
    }
}
