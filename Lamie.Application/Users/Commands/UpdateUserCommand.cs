using FluentValidation;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Users.Commands;

public sealed record UpdateUserCommand(
    Guid Id,
    string FullName,
    string? Phone,
    UserRole Role,
    bool IsActive) : IRequest;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Role).IsInEnum();
    }
}

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("User", request.Id);

        user.UpdateProfile(request.FullName, request.Phone);
        user.ChangeRole(request.Role);

        if (request.IsActive) user.Activate(); else user.Deactivate();

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}
