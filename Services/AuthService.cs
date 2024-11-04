using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using StudyCardsGenerator.Services.Interfaces;

namespace StudyCardsGenerator.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(
        ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider,
        IHttpClientFactory httpClientFactory)
    {
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }

    public async Task SetToken(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
        var httpClient = _httpClientFactory.CreateClient("API");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        (_authStateProvider as CustomAuthStateProvider)?.NotifyAuthenticationStateChanged();
    }

    public async Task RemoveToken()
    {
        await _localStorage.RemoveItemAsync("authToken");
        var httpClient = _httpClientFactory.CreateClient("API");
        httpClient.DefaultRequestHeaders.Authorization = null;
        (_authStateProvider as CustomAuthStateProvider)?.NotifyAuthenticationStateChanged();
    }
}