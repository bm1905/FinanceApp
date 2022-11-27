using FinancePlanner.API.Aggregator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Filters;
using FinancePlanner.Shared.Models.FinanceServices;
using System.Collections.Generic;

namespace FinancePlanner.API.Aggregator.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ValidateModelFilter]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [MapToApiVersion("1.0")]
    [HttpGet("Test")]
    [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
    public IActionResult Index()
    {
        return Ok(new { Status = "V1 Test Passed" });
    }

    [MapToApiVersion("1.0")]
    [HttpGet("GetPayInformation/{userId}")]
    [ProducesResponseType(typeof(ActionResult<List<PayInformationResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PayInformationResponse>>> GetPayInformation(string userId)
    {
        List<PayInformationResponse> response = await _financeService.GetPayList(userId, null);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpGet("GetPayInformation/{userId}/{payId:int}")]
    [ProducesResponseType(typeof(ActionResult<List<PayInformationResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PayInformationResponse>> GetPayInformation(string userId, int payId)
    {
        List<PayInformationResponse> response = await _financeService.GetPayList(userId, payId);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpPost("SavePayInformation")]
    [ProducesResponseType(typeof(ActionResult<PayInformationResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PayInformationResponse>> SavePayInformation([FromBody] PayInformationRequest request)
    {
        PayInformationResponse response = await _financeService.SavePay(request, null, null);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpPost("UpdatePayInformation/{userId}/{payId:int}")]
    [ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PayInformationResponse>> UpdatePayInformation([FromBody] PayInformationRequest request, string userId, int payId)
    {
        PayInformationResponse response = await _financeService.SavePay(request, userId, payId);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpDelete("DeletePayInformation/{userId}/{payId:int}")]
    [ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> DeletePayInformation(string userId, int payId)
    {
        bool response = await _financeService.DeletePay(userId, payId);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpGet("GetIncomeInformation/{userId}")]
    [ProducesResponseType(typeof(ActionResult<List<IncomeInformationResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<IncomeInformationResponse>>> GetIncomeInformation(string userId)
    {
        List<IncomeInformationResponse> response = await _financeService.GetIncomeList(userId, null);
        return Ok(response);
    }
}
