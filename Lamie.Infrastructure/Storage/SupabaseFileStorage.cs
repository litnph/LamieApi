using System.Net.Http.Headers;
using Lamie.Application.Common.Storage;
using Lamie.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Lamie.Infrastructure.Storage;

public sealed class SupabaseFileStorage : IFileStorage
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseOptions _options;

    public SupabaseFileStorage(HttpClient httpClient, IOptions<SupabaseOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> UploadPublicAsync(
        Stream content,
        string objectPath,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(objectPath)) throw new InvalidOperationException("Object path is required.");

        var baseUrl = _options.Url?.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(baseUrl)) throw new InvalidOperationException("Supabase Url is not configured.");
        if (string.IsNullOrWhiteSpace(_options.ServiceRoleKey)) throw new InvalidOperationException("Supabase ServiceRoleKey is not configured.");
        if (string.IsNullOrWhiteSpace(_options.StorageBucket)) throw new InvalidOperationException("Supabase StorageBucket is not configured.");

        var uploadUrl = $"{baseUrl}/storage/v1/object/{Uri.EscapeDataString(_options.StorageBucket)}/{EscapeObjectPath(objectPath)}";

        using var httpContent = new StreamContent(content);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/octet-stream");

        using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
        {
            Content = httpContent
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

        return $"{baseUrl}/storage/v1/object/public/{Uri.EscapeDataString(_options.StorageBucket)}/{EscapeObjectPath(objectPath)}";
    }

    private static string EscapeObjectPath(string objectPath)
    {
        var segments = objectPath
            .Replace('\\', '/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        return string.Join('/', segments.Select(Uri.EscapeDataString));
    }
}

