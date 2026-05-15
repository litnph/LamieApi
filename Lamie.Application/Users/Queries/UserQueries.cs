using Lamie.Application.Auth.Commands;
using Lamie.Application.Auth.Dtos;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Users.Queries;

public sealed record GetAllUsersQuery() : IRequest<List<AuthUserDto>>;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<AuthUserDto>;

public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<AuthUserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<AuthUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(TokenIssuer.ToDto).ToList();
    }
}

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, AuthUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("User", request.Id);

        return TokenIssuer.ToDto(user);
    }
}
