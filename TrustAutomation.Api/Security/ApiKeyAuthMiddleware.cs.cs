namespace TrustAutomation.Api.Security;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    public ApiKeyAuthMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, IConfiguration cfg)
    {
        // Protege apenas rotas /api/admin/*
        if (!ctx.Request.Path.StartsWithSegments("/api/admin"))
        {
            await _next(ctx);
            return;
        }

        var expected = cfg["ADMIN_API_KEY"];
        if (string.IsNullOrWhiteSpace(expected))
        {
            ctx.Response.StatusCode = 500;
            await ctx.Response.WriteAsync("ADMIN_API_KEY not configured.");
            return;
        }

        if (!ctx.Request.Headers.TryGetValue("x-api-key", out var provided) ||
            string.IsNullOrWhiteSpace(provided) ||
            !string.Equals(provided.ToString(), expected, StringComparison.Ordinal))
        {
            ctx.Response.StatusCode = 401;
            await ctx.Response.WriteAsync("Unauthorized.");
            return;
        }

        await _next(ctx);
    }
}
