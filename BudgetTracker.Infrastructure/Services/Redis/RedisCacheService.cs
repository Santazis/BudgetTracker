using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetTracker.Application.Interfaces.Redis;
using BudgetTracker.Infrastructure.Convertors;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BudgetTracker.Infrastructure.Services.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    private static readonly JsonSerializerSettings DeserializeOptions = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new PrivateResolver(),
        Converters = {new EmailJsonConvertor(),}
        
    };
    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetStringAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);
            if (value is null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value,DeserializeOptions);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SetStringAsync<T>(string key, T value)
    {
        try
        {
            var val = JsonConvert.SerializeObject(value,DeserializeOptions);
            await _cache.SetStringAsync(key, val);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}