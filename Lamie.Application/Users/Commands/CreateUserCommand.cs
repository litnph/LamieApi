using FluentValidation;
using Lamie.Application.Auth.Commands;
using Lamie.Application.Auth.Dtos;
using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Users.Commands;

public sealed record CreateUserCommand(
    string Email,
    string UserName,
    string Password,
    string FullName,
    string? Phone,
    UserRole Role,
    bool IsActive) : IRequest<AuthUserDto>;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Role).IsInEnum();
    }
}

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, AuthUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsEmailAsync(request.Email, null, cancellationToken))
            throw new ConflictException("Email is already in use");

        if (await _userRepository.ExistsUserNameAsync(request.UserName, null, cancellationToken))
            throw new ConflictException("UserName is already in use");

        var hash = _passwordHasher.Hash(request.Password);
        var user = new User(request.Email, request.UserName, hash, request.FullName, request.Role, request.Phone, request.IsActive);

        await _userRepository.AddAsync(user, cancellationToken);
        return TokenIssuer.ToDto(user);
    }
}
