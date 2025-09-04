using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.SavingGoal.Requests;
using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces;

public interface ISavingGoalService
{
    Task<Result> CreateAsync(CreateSavingGoal request, Guid userId, CancellationToken cancellation);
    Task<Result<SavingGoalDto>> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation);
}