using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;

    public BudgetService(IUnitOfWork unitOfWork, IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository)
    {
        _unitOfWork = unitOfWork;
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync(Guid userId, CancellationToken cancellation)
    {
        var budgets = await _budgetRepository.GetActiveBudgetsByUserIdAsync(userId, cancellation);
        if (budgets.Count == 0)
        {
            return new List<BudgetDto>();
        }

        var minDate = budgets.Min(b => b.Period.PeriodStart);
        var maxDate = budgets.Max(b => b.Period.PeriodEnd);

        TransactionFilter filter = new TransactionFilter()
        {
            From = minDate,
            To = maxDate,
            Categories = budgets.Select(b => b.CategoryId).ToHashSet(),
        };

        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId, filter, cancellation);

        var budgetDtos = budgets.GroupJoin(transactions, b => b.CategoryId, t => t.CategoryId,
            (b, t) =>
            {
               var transactions =  t.ToList();
                var spent = transactions
                    .Select(tx => tx.Amount)
                    .DefaultIfEmpty(Money.Create(0, "USD"))
                    .Aggregate((a, b) => a + b);
    
                var remainingAmount = b.LimitAmount - spent;
                return BudgetDto.FromEntity(b,transactions,spent,remainingAmount);
            });
        return budgetDtos;
    }

    public async Task<BudgetDto> CreateBudgetAsync(CreateBudget request, Guid userId, CancellationToken cancellation)
    {
        var period = BudgetPeriod.Create(request.PeriodStart, request.PeriodEnd);
        var amount = Money.Create(request.LimitAmount, request.Currency);
        var budget = Budget.Create(userId, request.Name, request.Description, request.CategoryId,
            amount, period);
        await _budgetRepository.AddAsync(budget, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return new BudgetDto(budget.Id, budget.Name, budget.Description, budget.LimitAmount,
            Money.Create(0, request.Currency), amount, PeriodStart: request.PeriodStart,
            PeriodEnd: request.PeriodEnd, IsExceeded: false, CreatedAt: budget.CreatedAt,
            Transactions: new List<BudgetTransactionDto>());
    }

    public async Task<BudgetDto> GetBudgetById(Guid id, Guid userId, CancellationToken cancellation)
    {
        var budget = await _budgetRepository.GetByIdAsync(id, userId, cancellation);
        if (budget is null)
        {
            throw new RequestException("Budget not found");
        }


        var transactions = await _transactionRepository.GetTransactionsForBudgetAsync(
            userId, budget.CategoryId, budget.Period, cancellation);
        var budgetTransactions = transactions.Select(t => new BudgetTransactionDto(
            TransactionId: t.Id,
            Amount: t.Amount,
            Description: t.Description,
            CreatedAt: t.CreatedAt)).ToList();
        var spentAmount = budgetTransactions.Select(t => t.Amount).Aggregate((a, b) => a + b);
        var remainingAmount = budget.LimitAmount - spentAmount;
        var isExceeded = budget.IsLimitExceeded(spentAmount);
        return new BudgetDto(budget.Id, budget.Name, budget.Description, budget.LimitAmount,
            spentAmount, remainingAmount, PeriodStart: budget.Period.PeriodStart,
            PeriodEnd: budget.Period.PeriodEnd, IsExceeded: isExceeded, CreatedAt: budget.CreatedAt,
            Transactions: budgetTransactions);
    }

    public async Task<BudgetDto> UpdateAsync(UpdateBudget request, Guid budgetId, Guid userId,
        CancellationToken cancellation)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId, userId, cancellation);
        if (budget is null)
        {
            throw new RequestException("Budget not found");
        }

        var limitAmount = Money.Create(request.LimitAmount, request.Currency);
        var period = BudgetPeriod.Create(request.PeriodStart, request.PeriodEnd);
        budget.Update(
            limitAmount: limitAmount,
            period: period,
            name: request.Name,
            description: request.Description);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return new BudgetDto(budget.Id, budget.Name, budget.Description, budget.LimitAmount,
            Money.Create(0, request.Currency), limitAmount, PeriodStart: request.PeriodStart,
            PeriodEnd: request.PeriodEnd, IsExceeded: false, CreatedAt: budget.CreatedAt,
            Transactions: new List<BudgetTransactionDto>());
    }
}