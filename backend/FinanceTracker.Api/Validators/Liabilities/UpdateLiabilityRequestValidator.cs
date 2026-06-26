using FinanceTracker.Api.DTOs.Liabilities;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Liabilities;

public class UpdateLiabilityRequestValidator : AbstractValidator<UpdateLiabilityRequest>
{
  public UpdateLiabilityRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.InterestRate)
      .GreaterThanOrEqualTo(0).WithMessage("InterstRate cannot be negative.")
      .LessThan(1).WithMessage("InterestRate must be expressed as decimal (e.g. 0.035 for 3.5%).");

    RuleFor(x => x.InstallmentAmount)
      .GreaterThan(0).WithMessage("InstallmentAmount must be greater than zero.");

    RuleFor(x => x.InstallmentFrequency)
      .IsInEnum().WithMessage("InstallmentFrequency must be a valid value.");

    RuleFor(x => x.EndDate)
      .NotEmpty().WithMessage("EndDate is required.");

    RuleFor(x => x.Pillar)
      .IsInEnum().WithMessage("Pillar must be a valid value.");

    RuleFor(x => x.Notes)
      .MaximumLength(500).WithMessage("Notes mus not exceed 500 characters.")
      .When(x => x.Notes is not null);
  }
}