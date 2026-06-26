using FinanceTracker.Api.DTOs.Transactions;
using FinanceTracker.Api.Models.Enums;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Transactions;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
  public CreateTransactionRequestValidator()
  {
    RuleFor(x => x.Type)
      .IsInEnum().WithMessage("Type must be a valid AccountType.");
    
    RuleFor(x => x.Amount)
      .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    RuleFor(x => x.Currency)
      .NotEmpty().WithMessage("Currency is required.")
      .Length(3).WithMessage("Currency must be a 3-letter ISO 4217 code (e.g. EUR).")
      .Matches("^[A-Z]{3}$").WithMessage("Currency must be uppercase letters only (e.g. EUR)");

    RuleFor(x => x.Date)
      .NotEmpty().WithMessage("Date is required.");

    RuleFor(x => x.Description)
      .NotEmpty().WithMessage("Description is required.")
      .MaximumLength(200).WithMessage("Description must not exceed 200 characters");

    RuleFor(x => x.AccountId)
      .NotEmpty().WithMessage("AccountId is required.")
      .NotEqual(Guid.Empty).WithMessage("AccountId must be a valid identifier.");

    RuleFor(x => x.CategoryId)
      .NotEqual(Guid.Empty).WithMessage("CategoryId must be a valid identifier.")
      .When(x => x.CategoryId is not null);

    RuleFor(x => x.LiabilityId)
      .NotEqual(Guid.Empty).WithMessage("LiabilityId must be a valid identifier.")
      .When(x => x.LiabilityId is not null);

    RuleFor(x => x.Notes)
      .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.")
      .When(x => x.Notes is not null);
  }
}