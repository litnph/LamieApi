using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Languages;

public sealed record CreateLanguageCommand(string Code, string Name, bool IsActive) : IRequest;

public sealed record UpdateLanguageCommand(string Code, string Name, bool IsActive) : IRequest;

public sealed record DeleteLanguageCommand(string Code) : IRequest;

public sealed class CreateLanguageHandler : IRequestHandler<CreateLanguageCommand>
{
    private readonly ILanguageRepository _repository;

    public CreateLanguageHandler(ILanguageRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            throw new ValidationException(new() { ["code"] = ["Code is required"] });
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(new() { ["name"] = ["Name is required"] });

        var exists = await _repository.ExistsAsync(request.Code, cancellationToken);
        if (exists)
            throw new ConflictException($"Language '{request.Code}' already exists.");

        var language = new Language(request.Code, request.Name, request.IsActive);
        await _repository.AddAsync(language, cancellationToken);
    }
}

public sealed class UpdateLanguageHandler : IRequestHandler<UpdateLanguageCommand>
{
    private readonly ILanguageRepository _repository;

    public UpdateLanguageHandler(ILanguageRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(new() { ["name"] = ["Name is required"] });

        var language = await _repository.GetByCodeAsync(request.Code, cancellationToken)
            ?? throw new NotFoundException("Language", request.Code);

        language.Rename(request.Name);
        language.SetActive(request.IsActive);

        await _repository.UpdateAsync(language, cancellationToken);
    }
}

public sealed class DeleteLanguageHandler : IRequestHandler<DeleteLanguageCommand>
{
    private readonly ILanguageRepository _repository;

    public DeleteLanguageHandler(ILanguageRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await _repository.GetByCodeAsync(request.Code, cancellationToken)
            ?? throw new NotFoundException("Language", request.Code);

        await _repository.DeleteAsync(language, cancellationToken);
    }
}
