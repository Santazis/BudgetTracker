using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Services;
using BudgetTracker.Application.Services.Auth;
using BudgetTracker.Application.Services.CategoryServices;
using BudgetTracker.Application.Services.TransactionServices;
using BudgetTracker.Application.Services.User;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Repositories;
using BudgetTracker.Infrastructure.Services;
using BudgetTracker.Infrastructure.Services.Jwt;

namespace BudgetTracker.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<IJwtAccessTokenService, JwtAccessTokenService>();
        services.AddScoped<IJwtRefreshTokenService, JwtRefreshTokenService>();
        services.AddScoped<IGetTokenClaimPrincipalService, GetTokenClaimPrincipalService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthRepository , AuthRepository>();
        services.AddScoped<IUserRepository , UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        return services;
    }

    public static IServiceCollection AddCrudServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ISummaryService, SummaryService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITransactionImportService, TransactionImportService>();
        return services;
    }
}