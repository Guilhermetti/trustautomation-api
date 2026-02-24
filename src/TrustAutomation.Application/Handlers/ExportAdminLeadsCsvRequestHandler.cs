using MediatR;
using TrustAutomation.Application.Queries.Leads;

namespace TrustAutomation.Application.Handlers
{
    public sealed class ExportAdminLeadsCsvRequestHandler : IRequestHandler<ExportAdminLeadsCsvQuery, string>
    {
        private readonly LeadHandler _leadHandler;

        public ExportAdminLeadsCsvRequestHandler(LeadHandler leadHandler)
            => _leadHandler = leadHandler;

        public Task<string> Handle(ExportAdminLeadsCsvQuery request, CancellationToken cancellationToken)
            => _leadHandler.Handle(request, cancellationToken);
    }
}
