using System.Net;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlanner.TaxServices.Services.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FederalTaxController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FederalTaxController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [MapToApiVersion("1.0")]
        [HttpGet("Test")]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
        public IActionResult Index()
        {
            return Ok(new { Status = "V1 Test Passed" });
        }

        [MapToApiVersion("1.0")]
        [HttpPost("CalculateFedTaxWithheld")]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FedTaxWithheldResponse>> CalculateFedTaxWithheld([FromBody] CalculateFedWithheldRequest request)
        {
            GetFedTaxWithheldQuery query = new GetFedTaxWithheldQuery(request);
            FedTaxWithheldResponse result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
