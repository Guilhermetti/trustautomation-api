using System.Text;
using System.Text.RegularExpressions;
using TrustAutomation.Application.Commands.Leads;
using TrustAutomation.Application.Interfaces;
using TrustAutomation.Application.Queries.Leads;
using TrustAutomation.Application.Results;
using TrustAutomation.Domain.Models;

namespace TrustAutomation.Application.Handlers
{
    public sealed class LeadHandler
    {
        private static readonly Regex EmailRegex =
            new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);

        private readonly ILeadRepository _repo;
        private readonly ISystemClock _clock;

        public LeadHandler(ILeadRepository repo, ISystemClock clock)
        {
            _repo = repo;
            _clock = clock;
        }

        public async Task<CreateLeadResult?> Handle(CreateLeadCommand request, CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(request.Honey))
                return null;

            var name = (request.Name ?? "").Trim();
            var email = (request.Email ?? "").Trim();
            var idea = (request.Idea ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(idea))
                throw new ValidationException("Campos obrigatórios: name, email, idea.");

            if (!request.Consent)
                throw new ValidationException("Consentimento é obrigatório.");

            if (!EmailRegex.IsMatch(email))
                throw new ValidationException("E-mail inválido.");

            var lead = Lead.Create(
                name: name,
                email: email,
                idea: idea,
                consent: true,
                company: string.IsNullOrWhiteSpace(request.Company) ? null : request.Company.Trim(),
                whatsapp: string.IsNullOrWhiteSpace(request.Whatsapp) ? null : request.Whatsapp.Trim(),
                needType: string.IsNullOrWhiteSpace(request.NeedType) ? null : request.NeedType.Trim(),
                deadline: string.IsNullOrWhiteSpace(request.Deadline) ? null : request.Deadline.Trim(),
                sourceUrl: string.IsNullOrWhiteSpace(request.SourceUrl) ? null : request.SourceUrl.Trim(),
                ip: string.IsNullOrWhiteSpace(request.Ip) ? null : request.Ip.Trim(),
                createdAtUtc: _clock.UtcNow
            );

            await _repo.AddAsync(lead, ct);
            await _repo.SaveChangesAsync(ct);

            return new CreateLeadResult(lead.Id, lead.CreatedAtUtc);
        }

        public Task<PagedResult<Lead>> Handle(GetAdminLeadsQuery request, CancellationToken ct)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Clamp(request.PageSize, 1, 200);

            return _repo.GetPagedAsync(page, pageSize, ct);
        }

        public async Task<string> Handle(ExportAdminLeadsCsvQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllForExportAsync(ct);

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

            return sb.ToString();
        }
    }
}
