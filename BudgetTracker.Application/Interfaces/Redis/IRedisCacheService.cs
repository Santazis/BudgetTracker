namespace BudgetTracker.Application.Interfaces.Redis;

public interface IRedisCacheService
{
    Task<T?> GetStringAsync<T>(string key);
    Task SetStringAsync<T>(string key,T value);
    Task RemoveAsync(string key);
}