using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.RateLimiting;
using TrustAutomation.Api.Data;
using TrustAutomation.Api.Dtos;
using TrustAutomation.Api.Models;
using TrustAutomation.Api.Security;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigin = builder.Configuration["ALLOWED_ORIGIN"];

// DB
var conn = builder.Configuration["DATABASE_URL"];
if (string.IsNullOrWhiteSpace(conn))
    throw new Exception("DATABASE_URL is required.");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(conn));

// CORS (fechado)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("frontend", p =>
        p.WithOrigins(allowedOrigin!)
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// Rate limit (básico) – por IP
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("leads", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("frontend");
app.UseRateLimiter();
app.UseMiddleware<ApiKeyAuthMiddleware>();

// Health
app.MapGet("/health", () => Results.Ok(new { ok = true, utc = DateTime.UtcNow }));

// POST público
app.MapPost("/api/leads", async (HttpContext ctx, AppDbContext db, LeadCreateRequest req) =>
{
    // Honeypot: se preencher, considera bot (retorna 200 para não incentivar)
    if (!string.IsNullOrWhiteSpace(req.Honey))
        return Results.Ok(new { ok = true });

    var name = (req.Name ?? "").Trim();
    var email = (req.Email ?? "").Trim();
    var idea = (req.Idea ?? "").Trim();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(idea))
        return Results.BadRequest("Campos obrigatórios: name, email, idea.");

    if (!req.Consent)
        return Results.BadRequest("Consentimento é obrigatório.");

    // validação simples de e-mail
    if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
        return Results.BadRequest("E-mail inválido.");

    // captura IP (melhor usar headers quando tiver proxy/CDN; Render geralmente injeta X-Forwarded-For)
    var ip = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? ctx.Connection.RemoteIpAddress?.ToString();

    var lead = new Lead
    {
        Name = name,
        Email = email,
        Company = string.IsNullOrWhiteSpace(req.Company) ? null : req.Company.Trim(),
        Whatsapp = string.IsNullOrWhiteSpace(req.Whatsapp) ? null : req.Whatsapp.Trim(),
        NeedType = string.IsNullOrWhiteSpace(req.NeedType) ? null : req.NeedType.Trim(),
        Deadline = string.IsNullOrWhiteSpace(req.Deadline) ? null : req.Deadline.Trim(),
        Idea = idea,
        Consent = true,
        SourceUrl = string.IsNullOrWhiteSpace(req.SourceUrl) ? null : req.SourceUrl.Trim(),
        Ip = string.IsNullOrWhiteSpace(ip) ? null : ip.Trim(),
        CreatedAtUtc = DateTime.UtcNow
    };

    db.Leads.Add(lead);
    await db.SaveChangesAsync();

    return Results.Created($"/api/admin/leads/{lead.Id}", new { id = lead.Id, createdAtUtc = lead.CreatedAtUtc });
})
.RequireRateLimiting("leads");

// ADMIN: listagem paginada (protegida por middleware)
app.MapGet("/api/admin/leads", async (AppDbContext db, int page = 1, int pageSize = 20) =>
{
    page = Math.Max(page, 1);
    pageSize = Math.Clamp(pageSize, 1, 200);

    var query = db.Leads.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc);

    var total = await query.CountAsync();
    var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    return Results.Ok(new { page, pageSize, total, items });
});

// ADMIN: export CSV
app.MapGet("/api/admin/leads/export.csv", async (AppDbContext db) =>
{
    var items = await db.Leads.AsNoTracking()
        .OrderByDescending(x => x.CreatedAtUtc)
        .ToListAsync();

    static string Esc(string? s)
    {
        s ??= "";
        s = s.Replace("\"", "\"\"");
        return $"\"{s}\"";
    }

    var sb = new StringBuilder();
    sb.AppendLine("Id,CreatedAtUtc,Name,Email,Company,Whatsapp,NeedType,Deadline,Idea,SourceUrl,Ip");

    foreach (var x in items)
    {
        sb.AppendLine(string.Join(",",
            x.Id,
            x.CreatedAtUtc.ToString("o"),
            Esc(x.Name),
            Esc(x.Email),
            Esc(x.Company),
            Esc(x.Whatsapp),
            Esc(x.NeedType),
            Esc(x.Deadline),
            Esc(x.Idea),
            Esc(x.SourceUrl),
            Esc(x.Ip)
        ));
    }

    return Results.Text(sb.ToString(), "text/csv; charset=utf-8");
});

app.Run();
