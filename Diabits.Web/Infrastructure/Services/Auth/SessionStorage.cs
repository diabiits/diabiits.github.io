using System.Text.Json;

using Microsoft.JSInterop;

namespace Diabits.Web.Infrastructure.Services.Auth;

/// <summary>
/// Manages JWT token persistence in browser's localStorage via JavaScript Interop.
/// Saves/loads/clears authentication tokens so they survive page refreshes.
/// </summary>
public class SessionStorage(IJSRuntime js)
{
    private readonly IJSRuntime _js = js;

    public async Task SaveAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await _js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> LoadAsync<T>(string key)
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", key);

        if (string.IsNullOrWhiteSpace(json)) return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }

    public async Task ClearAsync(string key) =>
        await _js.InvokeVoidAsync("localStorage.removeItem", key);
}