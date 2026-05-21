using Diabits.Web.DTOs;
using Diabits.Web.Infrastructure.Api;
using Diabits.Web.Models;

namespace Diabits.Web.Infrastructure.Services.Auth;

/// <summary>
/// Handles authentication business logic and state management.
/// </summary>
public class AuthService(ApiClient apiClient, JwtAuthStateProvider authStateProvider)
{
    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        var result = await apiClient.PostAsync<AuthResponse>("Auth/login", new LoginRequest(username, password));

        if (!result.IsSuccess)
            return AuthResult.Fail(result.Error ?? "Login failed");

        if (result.Data?.AccessToken == null)
            return AuthResult.Fail("Invalid response from server");

        await authStateProvider.SetSessionAsync(result.Data.AccessToken);

        return AuthResult.Success();
    }

    public async Task LogoutAsync()
    {
        await authStateProvider.ClearSessionAsync();
        // TODO: Call backend logout endpoint when implementing refresh tokens
    }

    public async Task<AuthResult> UpdateCredentialsAsync(string currentPassword, string? newUsername = null, string? newPassword = null)
    {
        var result = await apiClient.PutAsync<AuthResponse>("Auth/UpdateCredentials", new UpdateAccountRequest(currentPassword, newUsername, newPassword));

        if (!result.IsSuccess)
            return AuthResult.Fail(result.Error ?? "Update failed");

        if (result.Data?.AccessToken == null)
            return AuthResult.Fail("Invalid response from server");

        await authStateProvider.SetSessionAsync(result.Data.AccessToken);

        return AuthResult.Success();
    }
}