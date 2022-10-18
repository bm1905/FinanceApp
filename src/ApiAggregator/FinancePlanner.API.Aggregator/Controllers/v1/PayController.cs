using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Filters;
using FinancePlanner.API.Aggregator.Models;
using FinancePlanner.API.Aggregator.Services;
using Microsoft.AspNetCore.Http;

namespace FinancePlanner.API.Aggregator.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ValidateModelFilter]
    public class PayController : ControllerBase
    {
        private readonly IPayCheckService _payCheckService;

        public PayController(IPayCheckService payCheckService)
        {
            _payCheckService = payCheckService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Test")]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
        public IActionResult Index()
        {
            return Ok(new { Status = "V1 Test Passed" });
        }

        [MapToApiVersion("1.0")]
        [HttpPost("CalculatePayCheck")]
        [ProducesResponseType(typeof(ActionResult<PayCheckResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PayCheckResponse>> CalculateTotalTaxableWages([FromBody] PayCheckRequest request)
        {
            PayCheckResponse response = await _payCheckService.CalculatePayCheck(request);
            return Ok(response);
        }
    }
}
