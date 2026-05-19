using ExamSystem.Services.Interfaces;
using StackExchange.Redis;

namespace ExamSystem.Services.Implementations;

public class TokenBlacklistService(IConnectionMultiplexer _redis) : ITokenBlacklistService
{
    private readonly IDatabase _db = _redis.GetDatabase();

    public async Task AddToBlacklistAsync(string token, TimeSpan expiry) =>
        await _db.StringSetAsync(token, "blacklisted", expiry);

    public async Task<bool> IsBlacklistedAsync(string token) =>
        await _db.KeyExistsAsync(token);
}
