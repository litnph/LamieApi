using FluentValidation;

namespace Lamie.Application.Settings.Products.Commands;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);

        RuleFor(x => x.Sku)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price).GreaterThan(0);

        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);

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
