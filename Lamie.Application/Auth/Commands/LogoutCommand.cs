using Lamie.Application.Common.Auth;
using Lamie.Domain.Repositories;
using Lamie.Shared.Time;
using MediatR;

namespace Lamie.Application.Auth.Commands;

public sealed record LogoutCommand(string RefreshToken) : IRequest;

public sealed class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IJwtTokenService _tokenService;
    private readonly IClock _clock;

    public LogoutHandler(
        IRefreshTokenRepository refreshRepository,
        IJwtTokenService tokenService,
        IClock clock)
    {
        _refreshRepository = refreshRepository;
        _tokenService = tokenService;
        _clock = clock;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken)) return;

        var hash = _tokenService.HashRefreshToken(request.RefreshToken);
        var stored = await _refreshRepository.GetByHashAsync(hash, cancellationToken);
        if (stored is null || !stored.IsActive) return;

        stored.Revoke(_clock.UtcNow);
        await _refreshRepository.UpdateAsync(stored, cancellationToken);
    }
}
