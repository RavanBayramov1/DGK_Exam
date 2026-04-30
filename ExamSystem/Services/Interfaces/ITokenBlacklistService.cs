namespace ExamSystem.Services.Interfaces;

public interface ITokenBlacklistService
{
    Task AddToBlacklistAsync(string token, TimeSpan expiry);
    Task<bool> IsBlacklistedAsync(string token);
}