using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TrustAutomation.Api.Extensions;
using TrustAutomation.Application.Commands.Leads;
using TrustAutomation.Application.Results;

namespace TrustAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [EnableRateLimiting(RateLimitExtensions.LeadsPolicy)]
    public sealed class LeadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeadsController(IMediator mediator) => _mediator = mediator;

        public sealed record LeadCreateRequest(
            string Name,
            string Email,
            string? Company,
            string? Whatsapp,
            string? NeedType,
            string? Deadline,
            string Idea,
            bool Consent,
            string? SourceUrl,
            string? Honey
        );

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeadCreateRequest req, CancellationToken ct)
        {
            var ip = Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                var result = await _mediator.Send(new CreateLeadCommand(
                    Name: req.Name,
                    Email: req.Email,
                    Company: req.Company,
                    Whatsapp: req.Whatsapp,
                    NeedType: req.NeedType,
                    Deadline: req.Deadline,
                    Idea: req.Idea,
                    Consent: req.Consent,
                    SourceUrl: req.SourceUrl,
                    Ip: ip,
                    Honey: req.Honey
                ), ct);

                // honeypot => 200 ok (mesmo comportamento)
                if (result is null)
                    return Ok(new { ok = true });

                return Created($"/api/admin/leads/{result.Id}", new { id = result.Id, createdAtUtc = result.CreatedAtUtc });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
