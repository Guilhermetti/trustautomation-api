using MediatR;
using TrustAutomation.Application.Queries.Leads;
using TrustAutomation.Application.Results;
using TrustAutomation.Domain.Models;

namespace TrustAutomation.Application.Handlers
{
    public sealed class GetAdminLeadsRequestHandler : IRequestHandler<GetAdminLeadsQuery, PagedResult<Lead>>
    {
        private readonly LeadHandler _leadHandler;

        public GetAdminLeadsRequestHandler(LeadHandler leadHandler)
            => _leadHandler = leadHandler;

        public Task<PagedResult<Lead>> Handle(GetAdminLeadsQuery request, CancellationToken cancellationToken)
            => _leadHandler.Handle(request, cancellationToken);
    }
}
