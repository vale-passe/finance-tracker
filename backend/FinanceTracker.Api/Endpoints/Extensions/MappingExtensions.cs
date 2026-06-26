using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.DTOs.Categories;
using FinanceTracker.Api.DTOs.Liabilities;
using FinanceTracker.Api.DTOs.Transactions;
using FinanceTracker.Api.DTOs.Transfers;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Endpoints.Extensions;

public static class MappingExtensions
{
  public static AccountResponse ToResponse(this Account account) =>
    new(
      Id: account.Id,
      Name: account.Name,
      Type: account.Type,
      Pillar: account.Pillar,
      Currency: account.Currency,
      Balance: account.Balance,
      Color: account.Color,
      IsAchived: account.IsAchived,
      Notes: account.Notes,
      CreatedAt: account.CreatedAt,
      UpdatedAt: account.UpdatedAt
    );

  public static CategoryResponse ToResponse(this Category category) =>
    new(
      Id: category.Id,
      Name: category.Name,
      Icon: category.Icon,
      ParentCategoryId: category.ParentCategoryId,
      IsRoot: category.IsRoot,
      CreatedAt: category.CreatedAt,
      UpdatedAt: category.UpdatedAt
    )
    {
      Children = [.. category.Children.Select(c => c.ToResponse())]
    };

  public static TransactionResponse ToResponse(this Transaction transaction) =>
    new(
      Id: transaction.Id,
      Type: transaction.Type,
      Amount: transaction.Amount,
      Currency: transaction.Currency,
      Date: transaction.Date,
      Description: transaction.Description,
      AccountId: transaction.AccountId,
      CategoryId: transaction.CategoryId,
      LiabilityId: transaction.LiabilityId,
      IsCategorized: transaction.IsCategorized,
      Notes: transaction.Notes,
      CreatedAt: transaction.CreatedAt,
      UpdatedAt: transaction.UpdatedAt
    );

  public static TransferResponse ToResponse(this Transfer transfer) =>
    new(
      Id: transfer.Id,
      Type: transfer.Type,
      FromAccountId: transfer.FromAccountId,
      ToAccountId: transfer.ToAccountId,
      Amount: transfer.Amount,
      Currency: transfer.Currency,
      Date: transfer.Date,
      Description: transfer.Description,
      Notes: transfer.Notes,
      CreatedAt: transfer.CreatedAt,
      UpdatedAt: transfer.UpdatedAt
    );

  public static LiabiltyResponse ToResponse(this Liability liability) =>
    new(
      Id: liability.Id,
      Name: liability.Name,
      Type: liability.Type,
      AccountId: liability.AccountId,
      OriginalAmount: liability.OriginalAmount,
      RemainingAmount: liability.RemainingAmount,
      InterestRate: liability.InterestRate,
      InstallmentAmount: liability.InstallmentAmount,
      InstallmentFrequency: liability.InstallmentFrequency,
      StartDate: liability.StartDate,
      EndDate: liability.EndDate,
      Pillar: liability.Pillar,
      Notes: liability.Notes,
      CreatedAt: liability.CreatedAt,
      UpdatedAt: liability.UpdatedAt
    );
}