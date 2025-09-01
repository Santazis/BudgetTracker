using BudgetTracker.Application.Models.SavingGoal.Requests;
using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces;

public interface ISavingGoalService
{
    Task<Result> CreateAsync(CreateSavingGoal request, Guid userId, CancellationToken cancellation);
    
}