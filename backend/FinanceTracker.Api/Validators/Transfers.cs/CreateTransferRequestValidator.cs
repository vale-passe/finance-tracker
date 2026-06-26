using FinanceTracker.Api.DTOs.Transfers;
using FluentValidation;

namespace FinanceTracker.Api.Validators.Transfers;

public class CreateTransferRequestValidator : AbstractValidator<CreateTransferRequest>
{
  public CreateTransferRequestValidator()
  {
    RuleFor(x => x.Type)
      .IsInEnum().WithMessage("Type must be a valid AccountType.");

    RuleFor(x => x.FromAccountId)
      .NotEqual(Guid.Empty).WithMessage("FromAccountId must be a valid identifier.");

    RuleFor(x => x.ToAccountId)
      .NotEqual(Guid.Empty).WithMessage("ToAccountId must be a valid identifier.");

    RuleFor(x => x.ToAccountId)
      .NotEqual(x => x.FromAccountId).WithMessage("Source and destination must be different.");
    
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

    RuleFor(x => x.Notes)
      .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.")
      .When(x => x.Notes is not null);
  }
}