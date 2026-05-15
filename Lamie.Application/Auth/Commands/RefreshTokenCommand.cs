using FluentValidation;
using Lamie.Application.Auth.Dtos;
using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using Lamie.Shared.Time;
using MediatR;

namespace Lamie.Application.Auth.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResultDto>;

public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IJwtTokenService _tokenService;
    private readonly IClock _clock;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshRepository,
        IJwtTokenService tokenService,
        IClock clock)
    {
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _tokenService = tokenService;
        _clock = clock;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = _tokenService.HashRefreshToken(request.RefreshToken);
        var stored = await _refreshRepository.GetByHashAsync(hash, cancellationToken)
            ?? throw new UnauthorizedException("Invalid refresh token");

        if (!stored.IsActive)
            throw new UnauthorizedException("Refresh token is expired or revoked");

        var user = await _userRepository.GetByIdAsync(stored.UserId, cancellationToken)
            ?? throw new UnauthorizedException("Invalid refresh token");

        if (!user.IsActive)
            throw new UnauthorizedException("User is inactive");

        var newAccess = _tokenService.IssueAccessToken(user);
        var newRefresh = _tokenService.IssueRefreshToken();
        var newRefreshEntity = new Domain.Entities.Auth.RefreshToken(user.Id, newRefresh.TokenHash, newRefresh.ExpiresAt);
        await _refreshRepository.AddAsync(newRefreshEntity, cancellationToken);

        stored.Revoke(_clock.UtcNow, replacedBy: newRefreshEntity.Id);
        await _refreshRepository.UpdateAsync(stored, cancellationToken);

        return new AuthResultDto
        {
            User = TokenIssuer.ToDto(user),
            Tokens = new AuthTokensDto
            {
                AccessToken = newAccess.Token,
                AccessTokenExpiresAt = newAccess.ExpiresAt,
                RefreshToken = newRefresh.Token,
                RefreshTokenExpiresAt = newRefresh.ExpiresAt,
            }
        };
    }
}
