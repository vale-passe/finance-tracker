using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.Models.Enums;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Accounts;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
  public CreateAccountRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.Type)
      .IsInEnum().WithMessage("Type must be a valid AccountType.");

    // Pillar obbligatorio per tutti tranne che per Crypto e Investment,
    // dove il pilastro vive sulle Holdings
    RuleFor(x => x.Pillar)
      .NotNull().WithMessage("Pillar is rquired for this account type.")
      .When(x => x.Type is not AccountType.Investment and not AccountType.Crypto);

    RuleFor(x => x.Currency)
      .NotEmpty().WithMessage("Currency is required.")
      .Length(3).WithMessage("Currency must be a 3-letter ISO 4217 code (e.g. EUR).")
      .Matches("^[A-Z]{3}$").WithMessage("Currency must be uppercase letters only (e.g. EUR)");

    RuleFor(x => x.Color)
      .NotEmpty().WithMessage("Color is required.")
      .Matches("^#([A-Fa-f0-9]{6})$").WithMessage("Color must be a valid hex color (e.g. #0066CC)");

    RuleFor(x => x.Notes)
      .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.")
      .When(x => x.Notes is not null);
  }
}