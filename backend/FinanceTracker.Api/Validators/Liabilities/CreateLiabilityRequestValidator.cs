using FinanceTracker.Api.DTOs.Liabilities;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Liabilities;

public class CreateLiabilityRequestValidator : AbstractValidator<CreateLiabilityRequest>
{
  public CreateLiabilityRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.Type)
      .IsInEnum().WithMessage("Type must be a valid LiabilityType");

    RuleFor(x => x.AccountId)
      .NotEqual(Guid.Empty).WithMessage("AccountId must be a valid identifier.");

    RuleFor(x => x.OriginalAmount)
      .GreaterThan(0).WithMessage("OriginalAmount must be grater than zero.");

    RuleFor(x => x.InterestRate)
      .GreaterThanOrEqualTo(0).WithMessage("InterstRate cannot be negative.")
      .LessThan(1).WithMessage("InterestRate must be expressed as decimal (e.g. 0.035 for 3.5%).");

    RuleFor(x => x.InstallmentAmount)
      .GreaterThan(0).WithMessage("InstallmentAmount must be greater than zero.");

    RuleFor(x => x.InstallmentFrequency)
      .IsInEnum().WithMessage("InstallmentFrequency must be a valid value.");

    RuleFor(x => x.StartDate)
      .NotEmpty().WithMessage("StartDate is required.");

    RuleFor(x => x.EndDate)
      .NotEmpty().WithMessage("EndDate is required.")
      .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");

    RuleFor(x => x.Pillar)
      .IsInEnum().WithMessage("Pillar must be a valid value.");

    RuleFor(x => x.Notes)
      .MaximumLength(500).WithMessage("Notes mus not exceed 500 characters.")
      .When(x => x.Notes is not null);
  }
}