using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Diabits.Web.Infrastructure.Services.Auth;

//TODO Implement dialog that asks user to log back in when access token expires
public class JwtAuthStateProvider(SessionStorage storage) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    //TODO move to constants class
    private const string Key = "diabits_accesstoken";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await storage.LoadAsync<string>(Key);

        if (accessToken is null)
            return new AuthenticationState(Anonymous);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        if (token.ValidTo < DateTime.UtcNow)
        {
            await storage.ClearAsync(Key);
            return new AuthenticationState(Anonymous);
        }

        var identity = new ClaimsIdentity(token.Claims, authenticationType: "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
    public async Task SetSessionAsync(string accessToken)
    {
        await storage.SaveAsync(Key, accessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task ClearSessionAsync()
    {
        await storage.ClearAsync(Key);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}