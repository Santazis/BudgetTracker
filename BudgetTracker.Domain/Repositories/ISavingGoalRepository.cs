using BudgetTracker.Domain.Models.SavingGoal;

namespace BudgetTracker.Domain.Repositories;

public interface ISavingGoalRepository
{
    Task CreateAsync(SavingGoal savingGoal,CancellationToken cancellation);
    Task<SavingGoal?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
}