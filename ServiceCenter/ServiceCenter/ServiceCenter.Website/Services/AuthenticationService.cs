using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceCenter.Application.Dtos.ActivationCodes;
using ServiceCenter.Application.Dtos.Users;
using ServiceCenter.Application.Services.Base;
using ServiceCenter.Website.AuthProviders;
using ServiceCenter.Website.Dtos;
using ServiceCenter.Website.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ServiceCenter.Website.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(
        HttpClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<SResult> SendCode(string phoneNumber)
    {
        SendActivationCodeDto dto = new() { PhoneNumber = phoneNumber };

        HttpResponseMessage response = await _client.PostAsJsonAsync("v1/Users/Account/SendActivationCode", dto);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult<SendActivationCodeResultDto>>();

        if (!postResult.IsSuccess)
        {
            return SResult.Failure(postResult.Message);
        }

        return SResult.Success();
    }

    public async Task<SResult> RegisterOrLogin(UserLoginDto dto)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("v1/Users/Account/Login", dto);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult<LoginResultDto>>();

        if (!postResult.IsSuccess)
        {
            return SResult.Failure(postResult.Message);
        }

        await _localStorage.SetItemAsync("authToken", postResult.Data.Tokens.access_token.value);
        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(dto.PhoneNumber);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", postResult.Data.Tokens.access_token.value);

        return SResult.Success();
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.DefaultRequestHeaders.Authorization = null;
    }
}


