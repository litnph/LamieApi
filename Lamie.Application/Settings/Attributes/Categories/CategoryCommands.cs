using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Categories;

public sealed record CreateCategoryCommand(int SortOrder, bool IsActive, List<CategoryTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateCategoryCommand(Guid Id, int SortOrder, bool IsActive, List<CategoryTranslationInput> Translations) : IRequest;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateCategoryHandler(ICategoryRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations,
            t => t.LanguageCode,
            t => t.Name,
            _languageRepository,
            cancellationToken);

        var category = new Category(request.SortOrder, request.IsActive);

        foreach (var t in request.Translations)
        {
            category.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(category, cancellationToken);
        return category.Id;
    }
}

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateCategoryHandler(ICategoryRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations,
            t => t.LanguageCode,
            t => t.Name,
            _languageRepository,
            cancellationToken);

        var category = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Category", request.Id);

        category.Update(request.SortOrder, request.IsActive);

        foreach (var t in request.Translations)
        {
            category.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(category, cancellationToken);
    }
}

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Category", request.Id);

        await _repository.DeleteAsync(category, cancellationToken);
    }
}
