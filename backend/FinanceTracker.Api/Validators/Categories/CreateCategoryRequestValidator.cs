using FinanceTracker.Api.DTOs.Categories;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Categories;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
  public CreateCategoryRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.Icon)
      .MaximumLength(50).WithMessage("Icon must not exceed 50 characters.")
      .When(x => x.Icon is not null);

    // se viene fornito un ParentCategoryId, deve essere un Guid valido.
    // la verifica che il padre esista a db appartiene al service
    RuleFor(x => x.ParentCategoryId)
      .NotEqual(Guid.Empty).WithMessage("ParentCategoryId must be a valid identifier.")
      .When(x => x.ParentCategoryId is not null);

    // la verifica che il padre esista e che la profondità sia max 2
    // è logica di business che appartiene al service, non alla validazione dell'input
  }
}