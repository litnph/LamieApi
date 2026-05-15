using FluentValidation;
using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using Lamie.Shared.Auth;
using Lamie.Shared.Time;
using MediatR;

namespace Lamie.Application.Auth.Commands;

public sealed record ChangePasswordCommand(string CurrentPassword, string NewPassword) : IRequest;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
    }
}

public sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserContext _userContext;
    private readonly IClock _clock;

    public ChangePasswordHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshRepository,
        IPasswordHasher passwordHasher,
        IUserContext userContext,
        IClock clock)
    {
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _passwordHasher = passwordHasher;
        _userContext = userContext;
        _clock = clock;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.UserId is not Guid userId)
            throw new UnauthorizedException();

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedException();

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            throw new BusinessRuleException("Current password is incorrect");

        user.ChangePasswordHash(_passwordHasher.Hash(request.NewPassword));
        await _userRepository.UpdateAsync(user, cancellationToken);

        await _refreshRepository.RevokeAllForUserAsync(userId, _clock.UtcNow, cancellationToken);
    }
}
