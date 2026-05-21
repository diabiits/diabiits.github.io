using Diabits.Web.DTOs;
using Diabits.Web.Infrastructure.Api;
using Diabits.Web.Models;

namespace Diabits.Web.Infrastructure.Services.Invites;

/// <summary>
/// Handles authentication business logic and state management.
/// </summary>
public class InviteService(ApiClient apiClient)
{
    public async Task<List<Invite>> GetInvitesAsync()
    {
        return await apiClient.GetAsync<List<Invite>>("Invite") ?? [];
    }

    public async Task<Invite> CreateInviteAsync(string email)
    {
        return await apiClient.PostAsync<InviteRequest, Invite>("Invite", new InviteRequest(email)) 
            ?? throw new HttpRequestException("Something went wrong while creating invite");
    }
}