using ExamSystem.Services.Interfaces;
using StackExchange.Redis;

namespace ExamSystem.Services.Implementations;

public class TokenBlacklistService(IConnectionMultiplexer _redis) : ITokenBlacklistService
{
    public async Task AddToBlacklistAsync(string token, TimeSpan expiry)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(token, "blacklisted", expiry);
    }

    public async Task<bool> IsBlacklistedAsync(string token)
    {
        var db = _redis.GetDatabase();
        return await db.KeyExistsAsync(token);
    }
}
