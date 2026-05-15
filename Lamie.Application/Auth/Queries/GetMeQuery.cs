using Lamie.Application.Auth.Commands;
using Lamie.Application.Auth.Dtos;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using Lamie.Shared.Auth;
using MediatR;

namespace Lamie.Application.Auth.Queries;

public sealed record GetMeQuery() : IRequest<AuthUserDto>;

public sealed class GetMeHandler : IRequestHandler<GetMeQuery, AuthUserDto>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public GetMeHandler(IUserContext userContext, IUserRepository userRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<AuthUserDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        if (_userContext.UserId is not Guid userId)
            throw new UnauthorizedException();

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedException();

        return TokenIssuer.ToDto(user);
    }
}
