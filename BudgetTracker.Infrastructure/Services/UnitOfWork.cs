using System.Data;
using System.Data.Common;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace BudgetTracker.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellation)
    {
        return _context.SaveChangesAsync(cancellation);
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellation)
    {
        var transaction = await  _context.Database.BeginTransactionAsync(cancellation);
        return transaction.GetDbTransaction();
    }

    public Task CommitTransactionAsync(CancellationToken cancellation)
    {
        return _context.Database.CommitTransactionAsync(cancellation);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellation)
    {
        return _context.Database.RollbackTransactionAsync(cancellation);
    }
}