using Diabits.Web.DTOs;
using Diabits.Web.Infrastructure.Api;
using Diabits.Web.Models;

namespace Diabits.Web.Infrastructure.Services.Auth;

/// <summary>
/// Handles authentication business logic and state management.
/// </summary>
public class AuthService(ApiClient apiClient, JwtAuthStateProvider authStateProvider)
{
    public async Task LoginAsync(string username, string password)
    {
        var response = await apiClient.PostAsync<LoginRequest, AuthResponse>("Auth/login", new LoginRequest(username, password));

        if (response?.AccessToken == null)
            throw new HttpRequestException("Invalid response from server");

        await authStateProvider.SetSessionAsync(response.AccessToken);
    }

    public async Task LogoutAsync()
    {
        await authStateProvider.ClearSessionAsync();
        // TODO: Call backend logout endpoint when implementing refresh tokens
    }

    public async Task UpdateCredentialsAsync(string currentPassword, string? newUsername = null, string? newPassword = null)
    {
        var response = await apiClient.PutAsync<UpdateAccountRequest, AuthResponse>("Auth/UpdateCredentials", new UpdateAccountRequest(currentPassword, newUsername, newPassword));

        if (response?.AccessToken == null)
            throw new HttpRequestException("Invalid response from server");

        await authStateProvider.SetSessionAsync(response.AccessToken);
    }
}