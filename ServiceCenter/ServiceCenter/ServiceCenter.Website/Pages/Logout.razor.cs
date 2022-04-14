using Microsoft.AspNetCore.Components;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class Logout
{
    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.Logout();
        NavigationManager.NavigateTo("/");
    }
}
