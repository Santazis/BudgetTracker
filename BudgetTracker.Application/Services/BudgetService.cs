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
    private readonly ICategoryRepository _categoryRepository;
    public BudgetService(IUnitOfWork unitOfWork, IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync(Guid userId, CancellationToken cancellation)
    {
        var budgets = await _budgetRepository.GetActiveBudgetsByUserIdAsync(userId, cancellation);
        if (budgets.Count == 0)
        {
            return [];
        }
        var budgetDtos = new List<BudgetDto>();

        foreach (var budget in budgets)
        {
            var filter = new TransactionFilter()
            {
                From = budget.Period.PeriodStart,
                To = budget.Period.PeriodEnd,
                Categories = [budget.CategoryId]
            };
            
            var spent = await _transactionRepository.GetSpentAmountAsync(userId, filter, cancellation);
            var spentMoney = Money.Create(spent, budget.LimitAmount.Currency);
            var remaining = budget.LimitAmount - spentMoney;

            budgetDtos.Add(BudgetDto.FromEntity(budget, spentMoney, remaining));
        }

        return budgetDtos;
    }

    public async Task<BudgetDto> CreateBudgetAsync(CreateBudget request, Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, userId,cancellation);
        if (category is null)
        {
            throw new RequestException("Category not found");
        }
        var period = BudgetPeriod.Create(request.PeriodStart, request.PeriodEnd);
        var amount = Money.Create(request.LimitAmount, request.Currency);
        var budget = Budget.Create(userId, request.Name, request.Description, request.CategoryId,
            amount, period);
        await _budgetRepository.AddAsync(budget, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return new BudgetDto(budget.Id, budget.Name, budget.Description, budget.LimitAmount,
            Money.Create(0, request.Currency), amount, PeriodStart: request.PeriodStart,
            PeriodEnd: request.PeriodEnd, IsExceeded: false, CreatedAt: budget.CreatedAt);
    }

    public async Task<BudgetDto> GetBudgetById(Guid id, Guid userId, CancellationToken cancellation)
    {
        var budget = await _budgetRepository.GetByIdAsync(id, userId, cancellation);
        if (budget is null)
        {
            throw new RequestException("Budget not found");
        }

        var filter = new TransactionFilter()
        {
            From = budget.Period.PeriodStart,
            To = budget.Period.PeriodEnd,
            Categories = [budget.CategoryId]
        };
        var spent = await _transactionRepository.GetSpentAmountAsync(userId, filter, cancellation);
        var spentMoney = Money.Create(spent, budget.LimitAmount.Currency);
        var remaining = budget.LimitAmount - spentMoney;
        return BudgetDto.FromEntity(budget, spentMoney, remaining);
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
            PeriodEnd: request.PeriodEnd, IsExceeded: false, CreatedAt: budget.CreatedAt);
    }

    public async Task DeleteAsync(Guid budgetId, Guid userId, CancellationToken cancellation)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId, userId,cancellation);
        if (budget is null)
        {
            throw new RequestException("Budget not found");
        }
        _budgetRepository.Delete(budget);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }
}