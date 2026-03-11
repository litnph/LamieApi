using System.Net;
using Microsoft.AspNetCore.Http;
using Lamie.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Lamie.API.Services;

public sealed class SupabaseStorageService : IObjectStorageService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseOptions _options;

    public SupabaseStorageService(HttpClient httpClient, IOptions<SupabaseOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> UploadPublicAsync(IFormFile file, string objectPath, CancellationToken cancellationToken)
    {
        if (file.Length <= 0) throw new InvalidOperationException("File is empty.");
        if (string.IsNullOrWhiteSpace(objectPath)) throw new InvalidOperationException("Object path is required.");

        var baseUrl = _options.Url?.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(baseUrl)) throw new InvalidOperationException("Supabase Url is not configured.");
        if (string.IsNullOrWhiteSpace(_options.ServiceRoleKey)) throw new InvalidOperationException("Supabase ServiceRoleKey is not configured.");
        if (string.IsNullOrWhiteSpace(_options.StorageBucket)) throw new InvalidOperationException("Supabase StorageBucket is not configured.");

        // Upload API: POST {SUPABASE_URL}/storage/v1/object/{bucket}/{objectPath}
        var uploadUrl = $"{baseUrl}/storage/v1/object/{Uri.EscapeDataString(_options.StorageBucket)}/{EscapeObjectPath(objectPath)}";

        using var stream = file.OpenReadStream();
        using var content = new StreamContent(stream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");

        using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
        {
            Content = content
        };

        request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_options.ServiceRoleKey}");
        request.Headers.TryAddWithoutValidation("apikey", _options.ServiceRoleKey);
        request.Headers.TryAddWithoutValidation("x-upsert", "true");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Supabase upload failed: {(int)response.StatusCode} {response.StatusCode}. {body}");
        }

        // Public URL convention (bucket must be public)
        return $"{baseUrl}/storage/v1/object/public/{Uri.EscapeDataString(_options.StorageBucket)}/{EscapeObjectPath(objectPath)}";
    }

    private static string EscapeObjectPath(string objectPath)
    {
        // Supabase expects path segments separated by '/'. We escape each segment.
        var segments = objectPath
            .Replace('\\', '/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        return string.Join('/', segments.Select(Uri.EscapeDataString));
    }
}

