using FluentValidation;
using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using Lamie.Shared.Time;
using MediatR;

namespace Lamie.Application.Users.Commands;

public sealed record ResetPasswordCommand(Guid UserId, string NewPassword) : IRequest;

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
    }
}

public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IClock _clock;

    public ResetPasswordHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshRepository,
        IPasswordHasher passwordHasher,
        IClock clock)
    {
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _passwordHasher = passwordHasher;
        _clock = clock;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User", request.UserId);

        user.ChangePasswordHash(_passwordHasher.Hash(request.NewPassword));
        await _userRepository.UpdateAsync(user, cancellationToken);

        await _refreshRepository.RevokeAllForUserAsync(user.Id, _clock.UtcNow, cancellationToken);
    }
}
