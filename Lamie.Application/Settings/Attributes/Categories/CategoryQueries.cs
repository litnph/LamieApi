using AutoMapper;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Categories;

public sealed record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;

public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);
        return categories.Select(_mapper.Map<CategoryDto>).ToList();
    }
}

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Category", request.Id);

        return _mapper.Map<CategoryDto>(category);
    }
}
