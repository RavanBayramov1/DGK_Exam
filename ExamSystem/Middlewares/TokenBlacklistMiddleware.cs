using ExamSystem.Services.Interfaces;

namespace ExamSystem.Middlewares;

public class TokenBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        var token = context.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && await blacklistService.IsBlacklistedAsync(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Token etibarsızdır." });
            return;
        }

        await next(context);
    }
}
