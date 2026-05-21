using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Diabits.Web.Infrastructure.Services.Auth;

namespace Diabits.Web.Infrastructure.Api;

/// <summary>
/// Generic API client that handles all HTTP communication. Automatically includes JWT tokens via AuthorizationHandler.
/// </summary>
public class ApiClient(HttpClient http, JwtAuthStateProvider authProvider)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
    {
        var response = await http.GetAsync(endpoint, ct);

        await EnsureSuccessAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync(endpoint, request, JsonOptions, ct);

        await EnsureSuccessAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken ct = default)
    {
        var response = await http.PutAsJsonAsync(endpoint, request, JsonOptions, ct);

        await EnsureSuccessAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    public async Task DeleteAsync(string endpoint, CancellationToken ct = default)
    {
        var response = await http.DeleteAsync(endpoint, ct);
        await EnsureSuccessAsync(response, ct);
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await authProvider.ClearSessionAsync();
        }

        var error = await ReadErrorAsync(response, ct);

        throw new HttpRequestException(error);
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(error) ? "Request failed" : error;
        }
        catch
        {
            return "Request failed";
        }
    }
}
