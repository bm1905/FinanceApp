using Microsoft.AspNetCore.Mvc;
using System.Net;
using FinancePlanner.PreTaxServices.Services.Filters;
using FinancePlanner.PreTaxServices.Services.Models.DTOs;
using FinancePlanner.PreTaxServices.Services.Services;
using Microsoft.AspNetCore.Http;

namespace FinancePlanner.PreTaxServices.Services.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ValidateModelFilter]
    public class PreTaxController : ControllerBase
    {
        private readonly IPreTaxService _preTaxService;

        public PreTaxController(IPreTaxService preTaxService)
        {
            _preTaxService = preTaxService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Test")]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
        public IActionResult Index()
        {
            return Ok(new { Status = "V1 Test Passed" });
        }

        [MapToApiVersion("1.0")]
        [HttpPost("CalculateTotalTaxableWages")]
        [ProducesResponseType(typeof(ActionResult<PreTaxWagesResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PreTaxWagesResponse> CalculateTotalTaxableWages([FromBody] PreTaxWagesRequest request)
        {
            PreTaxWagesResponse response = _preTaxService.CalculateTaxableWages(request);
            return Ok(response);
        }
    }
}
