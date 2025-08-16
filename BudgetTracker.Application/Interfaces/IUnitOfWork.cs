namespace BudgetTracker.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellation);
    Task BeginTransactionAsync(CancellationToken cancellation);
    Task CommitTransactionAsync(CancellationToken cancellation);
    Task RollbackTransactionAsync(CancellationToken cancellation);
}