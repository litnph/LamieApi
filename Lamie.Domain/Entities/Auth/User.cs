using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities.Auth;

public class User : Entity
{
    public string Email { get; private set; } = default!;
    public string UserName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public string? Phone { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(
        string email,
        string userName,
        string passwordHash,
        string fullName,
        UserRole role,
        string? phone = null,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("UserName is required");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("PasswordHash is required");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("FullName is required");

        Email = email.Trim().ToLowerInvariant();
        UserName = userName.Trim();
        PasswordHash = passwordHash;
        FullName = fullName.Trim();
        Role = role;
        Phone = phone;
        IsActive = isActive;
    }

    public void UpdateProfile(string fullName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("FullName is required");

        FullName = fullName.Trim();
        Phone = phone;
    }

    public void ChangePasswordHash(string newHash)
    {
        if (string.IsNullOrWhiteSpace(newHash))
            throw new DomainException("Password hash is required");

        PasswordHash = newHash;
    }

    public void ChangeRole(UserRole role) => Role = role;

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void RecordLogin(DateTime utcNow) => LastLoginAt = utcNow;
}
