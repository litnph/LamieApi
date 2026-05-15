using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Users.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequest;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("User", request.Id);

        await _userRepository.DeleteAsync(user, cancellationToken);
    }
}
