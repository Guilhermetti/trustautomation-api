using Microsoft.EntityFrameworkCore;
using TrustAutomation.Application.Interfaces;
using TrustAutomation.Application.Results;
using TrustAutomation.Domain.Models;
using TrustAutomation.Infrastructure.Data;

namespace TrustAutomation.Infrastructure.Repositories
{
    public sealed class LeadRepository : ILeadRepository
    {
        private readonly AppDbContext _db;

        public LeadRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Lead lead, CancellationToken ct)
        {
            _db.Leads.Add(lead);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<PagedResult<Lead>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
        {
            var query = _db.Leads
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAtUtc);

            var total = await query.CountAsync(ct);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return new PagedResult<Lead>(page, pageSize, total, items);
        }

        public Task<List<Lead>> GetAllForExportAsync(CancellationToken ct)
        {
            return _db.Leads
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync(ct);
        }
    }
}
