using System.Data;
using System.Data.Common;

namespace BudgetTracker.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellation);
    Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellation);

}