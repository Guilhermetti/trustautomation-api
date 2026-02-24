using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrustAutomation.Application.Queries.Leads;

namespace TrustAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/admin/leads")]
    public sealed class AdminLeadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminLeadsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetAdminLeadsQuery(page, pageSize), ct);
            return Ok(new { result.Page, result.PageSize, result.Total, items = result.Items });
        }

        [HttpGet("export.csv")]
        public async Task<IActionResult> ExportCsv(CancellationToken ct)
        {
            var csv = await _mediator.Send(new ExportAdminLeadsCsvQuery(), ct);
            return Content(csv, "text/csv; charset=utf-8");
        }
    }
}
