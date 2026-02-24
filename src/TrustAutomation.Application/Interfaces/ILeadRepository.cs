using TrustAutomation.Application.Results;
using TrustAutomation.Domain.Models;

namespace TrustAutomation.Application.Interfaces
{
    public interface ILeadRepository
    {
        Task AddAsync(Lead lead, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct);

        Task<PagedResult<Lead>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<List<Lead>> GetAllForExportAsync(CancellationToken ct);
    }
}
