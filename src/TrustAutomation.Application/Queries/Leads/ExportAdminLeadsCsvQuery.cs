using MediatR;

namespace TrustAutomation.Application.Queries.Leads
{
    public sealed record ExportAdminLeadsCsvQuery() : IRequest<string>;
}
