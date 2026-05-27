using Inertia.Core;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Inertia.React;

public class ReactSSRRenderer : ISSRRenderer
{
    private readonly HttpClient _httpClient;
    private readonly string? _ssrEndpoint;

    public ReactSSRRenderer(HttpClient httpClient, InertiaOptions options)
    {
        _httpClient = httpClient;
        _ssrEndpoint = options.SSREndpoint;
    }

    public async Task<string?> RenderAsync(Page page)
    {
        if (string.IsNullOrEmpty(_ssrEndpoint))
            return null;

        try
        {
            var response = await _httpClient.PostAsJsonAsync(_ssrEndpoint, page, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SSRResponse>();
                return result?.Body;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SSR rendering failed: {ex.Message}");
        }

        return null;
    }
}

public class SSRResponse
{
    public string? Body { get; set; }
    public string? Head { get; set; }
}
