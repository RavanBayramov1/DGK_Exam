using ExamSystem.Services.Interfaces;

namespace ExamSystem.Middlewares;

public class TokenBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (!string.IsNullOrEmpty(token) && await blacklistService.IsBlacklistedAsync(token))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Token etibarsızdır (Blacklisted)." });
                return;
            }
        }

        await next(context);
    }
}
