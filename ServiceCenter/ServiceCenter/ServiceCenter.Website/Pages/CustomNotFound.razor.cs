﻿using Microsoft.AspNetCore.Components;

namespace ServiceCenter.Website.Pages;

public partial class CustomNotFound
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public void NavigateToHome()
    {
        NavigationManager.NavigateTo("/");
    }
}

