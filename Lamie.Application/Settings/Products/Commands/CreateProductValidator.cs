using FluentValidation;

namespace Lamie.Application.Settings.Products.Commands;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Translations)
            .NotEmpty();

        RuleForEach(x => x.Translations)
            .ChildRules(t =>
            {
                t.RuleFor(x => x.LanguageCode).NotEmpty();
                t.RuleFor(x => x.Name).NotEmpty();
                t.RuleFor(x => x.Slug).NotEmpty();
            });

        RuleForEach(x => x.Images)
            .ChildRules(i =>
            {
                i.RuleFor(x => x)
                    .Must(img =>
                        !string.IsNullOrWhiteSpace(img.ImageUrl) ||
                        (img.ImageFile is { Length: > 0 }))
                    .WithMessage("Either ImageUrl or ImageFile is required for each image.");
            });
    }
}
