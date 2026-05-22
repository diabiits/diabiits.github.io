using System.Net.Http.Headers;

using Diabits.Web.Infrastructure.Services.Auth;

namespace Diabits.Web.Infrastructure.Api;

//TODO Use storage directly here? Or use JwtAuthStateProvider?
/// <summary>
/// HTTP message handler that automatically attaches JWT bearer token to requests.
/// </summary>
public class AuthorizationHandler(SessionStorage storage) : DelegatingHandler
{
    //TODO move to constants class
    private const string Key = "diabits_accesstoken";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var accessToken = await storage.LoadAsync<string>(Key);
        if (accessToken is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, ct);
    }
}
