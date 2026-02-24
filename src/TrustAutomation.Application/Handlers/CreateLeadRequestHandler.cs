using MediatR;
using TrustAutomation.Application.Commands.Leads;
using TrustAutomation.Application.Results;

namespace TrustAutomation.Application.Handlers
{
    public sealed class CreateLeadRequestHandler : IRequestHandler<CreateLeadCommand, CreateLeadResult?>
    {
        private readonly LeadHandler _leadHandler;

        public CreateLeadRequestHandler(LeadHandler leadHandler)
            => _leadHandler = leadHandler;

        public Task<CreateLeadResult?> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
            => _leadHandler.Handle(request, cancellationToken);
    }
}
