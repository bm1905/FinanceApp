using FinancePlanner.API.Aggregator.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinancePlanner.API.Aggregator.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ValidateModelFilter]
public class FinanceController : ControllerBase
{
    [MapToApiVersion("2.0")]
    [HttpGet("Test")]
    [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
    public IActionResult Index()
    {
        return Ok(new { Status = "V2 Test Passed" });
    }
}