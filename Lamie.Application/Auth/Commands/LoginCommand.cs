using FluentValidation;
using Lamie.Application.Auth.Dtos;
using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using Lamie.Shared.Time;
using MediatR;

namespace Lamie.Application.Auth.Commands;

public sealed record LoginCommand(string Login, string Password) : IRequest<AuthResultDto>;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class LoginHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _tokenService;
    private readonly IClock _clock;

    public LoginHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService tokenService,
        IClock clock)
    {
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _clock = clock;
    }

    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailOrUserNameAsync(request.Login, cancellationToken)
            ?? throw new UnauthorizedException("Invalid credentials");

        if (!user.IsActive)
            throw new UnauthorizedException("User is inactive");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");

        user.RecordLogin(_clock.UtcNow);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return await TokenIssuer.IssueAsync(
            user, _tokenService, _refreshRepository, cancellationToken);
    }
}

internal static class TokenIssuer
{
    public static async Task<AuthResultDto> IssueAsync(
        User user,
        IJwtTokenService tokenService,
        IRefreshTokenRepository refreshRepository,
        CancellationToken cancellationToken)
    {
        var access = tokenService.IssueAccessToken(user);
        var refresh = tokenService.IssueRefreshToken();

        var token = new RefreshToken(user.Id, refresh.TokenHash, refresh.ExpiresAt);
        await refreshRepository.AddAsync(token, cancellationToken);

        return new AuthResultDto
        {
            User = ToDto(user),
            Tokens = new AuthTokensDto
            {
                AccessToken = access.Token,
                AccessTokenExpiresAt = access.ExpiresAt,
                RefreshToken = refresh.Token,
                RefreshTokenExpiresAt = refresh.ExpiresAt,
            }
        };
    }

    public static AuthUserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        UserName = user.UserName,
        FullName = user.FullName,
        Phone = user.Phone,
        Role = user.Role,
        IsActive = user.IsActive,
        LastLoginAt = user.LastLoginAt,
        CreatedAt = user.CreatedAt,
    };
}
