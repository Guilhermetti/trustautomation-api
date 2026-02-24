using System.Threading.RateLimiting;
using TrustAutomation.Api.Extensions;
using TrustAutomation.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigin = builder.Configuration["ALLOWED_ORIGIN"];
if (string.IsNullOrWhiteSpace(allowedOrigin))
    throw new Exception("ALLOWED_ORIGIN is required.");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithApiKey();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("frontend", p =>
        p.WithOrigins(allowedOrigin!)
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("leads", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});

builder.Services.AddTrustAutomation(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("frontend");

app.UseRateLimiter();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.MapControllers();

app.Run();