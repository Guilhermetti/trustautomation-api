using MediatR;
using TrustAutomation.Application.Results;

namespace TrustAutomation.Application.Commands.Leads
{
    public sealed record CreateLeadCommand(
        string Name,
        string Email,
        string? Company,
        string? Whatsapp,
        string? NeedType,
        string? Deadline,
        string Idea,
        bool Consent,
        string? SourceUrl,
        string? Ip,
        string? Honey
    ) : IRequest<CreateLeadResult?>;
}
