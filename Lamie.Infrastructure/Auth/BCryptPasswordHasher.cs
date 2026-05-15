using Lamie.Application.Common.Auth;

namespace Lamie.Infrastructure.Auth;

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash)
    {
        if (string.IsNullOrEmpty(hash)) return false;
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}
