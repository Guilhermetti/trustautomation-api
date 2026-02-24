using MediatR;
using TrustAutomation.Application.Results;
using TrustAutomation.Domain.Models;

namespace TrustAutomation.Application.Queries.Leads
{
    public sealed record GetAdminLeadsQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<Lead>>;
}
