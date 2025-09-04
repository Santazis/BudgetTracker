using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Redis;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITagRepository _tagRepository;
    private readonly IRedisCacheService _redisCacheService;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository, IRedisCacheService redisCacheService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tagRepository = tagRepository;
        _redisCacheService = redisCacheService;
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid userId, CancellationToken cancellation)
    {
        string key = $"user:{userId}";
        var userDtoCache = await _redisCacheService.GetStringAsync<UserDto>(key);
        if (userDtoCache is null)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellation);
            if (user is null)
            {
                return Result<UserDto>.Failure(UserErrors.UserNotFound);
            }
            userDtoCache = UserDto.FromEntity(user);
            await _redisCacheService.SetStringAsync(key, userDtoCache);
        }
        return Result<UserDto>.Success(userDtoCache);
    }

    public async Task<Result> AddPaymentMethod(Guid userId, CreatePaymentMethod request, CancellationToken cancellation)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
        {
            return Result<UserDto>.Failure(UserErrors.UserNotFound);
        }

        user.AddPaymentMethod(request.Type, request.Name, request.Details);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result> DeletePaymentMethodAsync(Guid userId, Guid paymentMethodId, CancellationToken cancellation)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
        {
            return Result<UserDto>.Failure(UserErrors.UserNotFound);
        }

        user.DeletePaymentMethod(paymentMethodId);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result> UpdatePaymentMethodAsync(Guid userId, Guid paymentMethodId, UpdatePaymentMethod request,
        CancellationToken cancellation)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
        {
            return Result<UserDto>.Failure(UserErrors.UserNotFound);
        }

        user.UpdatePaymentMethod(paymentMethodId, request.Name, request.Details, request.Type);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<IEnumerable<Tag>> GetUserTagsAsync(Guid userId, CancellationToken cancellation)
    {
        var tags = await _tagRepository.GetUserTagsAsync(userId, cancellation);
        return tags;
    }
}