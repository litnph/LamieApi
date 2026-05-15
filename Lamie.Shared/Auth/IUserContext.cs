namespace Lamie.Shared.Auth;

/// <summary>
/// Resolves the authenticated user for the current request scope.
/// Implementations live in Infrastructure (HttpContext) and tests.
/// </summary>
public interface IUserContext
{
    Guid? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
