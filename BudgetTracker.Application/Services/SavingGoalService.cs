using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.SavingGoal.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.SavingGoal;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services;

public class SavingGoalService : ISavingGoalService
{
    private readonly ISavingGoalRepository _savingGoalRepository;
    private readonly IUnitOfWork _unitOfWork;
    public SavingGoalService(ISavingGoalRepository savingGoalRepository, IUnitOfWork unitOfWork)
    {
        _savingGoalRepository = savingGoalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateAsync(CreateSavingGoal request, Guid userId, CancellationToken cancellation)
    {
        var period = BudgetPeriod.Create(request.PeriodStart,request.PeriodEnd);
        var targetAmount = Money.Create(request.TargetAmount, request.Currency);
        var savingGoal = SavingGoal.Create(userId, request.Name, targetAmount, period, request.Description);
        await _savingGoalRepository.CreateAsync(savingGoal, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result<SavingGoalDto>> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation)
    {
        var savingGoal = await _savingGoalRepository.GetByIdAsync(id, userId, cancellation);
        if (savingGoal is null)
        {
            return Result<SavingGoalDto>.Failure(SavingGoalErrors.SavingGoalNotFound);
        }
        return Result<SavingGoalDto>.Success(SavingGoalDto.FromEntity(savingGoal));
    }
}