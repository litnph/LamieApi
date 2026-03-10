using Microsoft.AspNetCore.Http;

namespace Lamie.API.Services;

public interface IObjectStorageService
{
    Task<string> UploadPublicAsync(IFormFile file, string objectPath, CancellationToken cancellationToken);
}

