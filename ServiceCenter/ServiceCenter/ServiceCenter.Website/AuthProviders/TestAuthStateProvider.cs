﻿using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ServiceCenter.Website.AuthProviders;

public class TestAuthStateProvider : AuthenticationStateProvider
{
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await Task.Delay(1500);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Role, "Administrator")
        };

        var anonymous = new ClaimsIdentity(claims, "testAuthType");
        anonymous = new ClaimsIdentity();

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
    }
}