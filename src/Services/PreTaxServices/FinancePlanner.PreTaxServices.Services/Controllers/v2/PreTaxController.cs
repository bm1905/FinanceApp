using Microsoft.AspNetCore.Mvc;
using System.Net;
using FinancePlanner.PreTaxServices.Services.Filters;

namespace FinancePlanner.PreTaxServices.Services.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ValidateModelFilter]
    public class PreTaxController : ControllerBase
    {
        [MapToApiVersion("2.0")]
        [HttpGet("Test")]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
        public IActionResult Index()
        {
            return Ok(new { Status = "V2 Test Passed" });
        }
    }
}
