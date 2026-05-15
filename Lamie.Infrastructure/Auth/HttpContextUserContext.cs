using System.Security.Claims;
using Lamie.Shared.Auth;
using Microsoft.AspNetCore.Http;

namespace Lamie.Infrastructure.Auth;

public sealed class HttpContextUserContext : IUserContext
{
    private readonly IHttpContextAccessor _accessor;

    public HttpContextUserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var raw = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? Principal?.FindFirstValue("sub");
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public string? UserName => Principal?.FindFirstValue(ClaimTypes.Name);
    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);
    public string? Role => Principal?.FindFirstValue(ClaimTypes.Role);
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
}
