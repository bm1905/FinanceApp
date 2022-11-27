using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Extensions;
using FinancePlanner.API.Aggregator.Models;
using FinancePlanner.Shared.Models.Common;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.FinanceServices;
using FinancePlanner.Shared.Models.WageServices;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.API.Aggregator.Services;

public class FinanceService : IFinanceService
{
    private readonly IPayCheckService _payCheckService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public FinanceService(IPayCheckService payCheckService, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _payCheckService = payCheckService;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<List<PayInformationResponse>> GetPayList(string userId, int? payId)
    {
        HttpClient financeServiceClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:FinanceServiceClient:ClientName").Value);
        string financeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:GetPayInformation").Value}/{userId}";
        if (payId != null)
        {
            financeServiceUrl += $"/{payId}";
        }
        List<PayInformationResponse> payInformationResponse = await financeServiceClient.GetList<PayInformationResponse>(financeServiceUrl);
        return payInformationResponse;
    }

    public async Task<List<IncomeInformationResponse>> GetIncomeList(string userId, int? incomeId)
    {
        HttpClient financeServiceClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:FinanceServiceClient:ClientName").Value);
        string financeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:GetIncomeInformation").Value}/{userId}";
        if (incomeId != null)
        {
            financeServiceUrl += $"/{incomeId}";
        }
        List<IncomeInformationResponse> incomeInformationResponse = await financeServiceClient.GetList<IncomeInformationResponse>(financeServiceUrl);
        return incomeInformationResponse;
    }

    public async Task<PayInformationResponse> SavePay(PayInformationRequest request, string? userId, int? payId)
    {
        string financeServiceUrl;
        HttpClient financeServiceClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:FinanceServiceClient:ClientName").Value);
        if (userId != null && payId != null)
        {
            financeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:UpdatePayInformation").Value}/{userId}/{payId}";
        }
        else
        {
            financeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:SavePayInformation").Value}";
        }

        PayInformationResponse? payInformationResponse = await financeServiceClient.Post<PayInformationRequest, PayInformationResponse>(request, financeServiceUrl);
        if (payInformationResponse == null)
        {
            throw new InternalServerErrorException($"Something went wrong when calling {financeServiceUrl}, received null response");
        }

        List<WeeklyHoursAndRateDto> weeklyHoursAndRate = new()
        {
            new WeeklyHoursAndRateDto()
            {
                HourlyRate = payInformationResponse.BiWeeklyHoursAndRate.HourlyRate,
                TimeOffHours = payInformationResponse.BiWeeklyHoursAndRate.Week1TimeOffHours,
                TotalHours = payInformationResponse.BiWeeklyHoursAndRate.Week1TimeOffHours
            },
            new WeeklyHoursAndRateDto()
            {
                HourlyRate = payInformationResponse.BiWeeklyHoursAndRate.HourlyRate,
                TimeOffHours = payInformationResponse.BiWeeklyHoursAndRate.Week2TimeOffHours,
                TotalHours = payInformationResponse.BiWeeklyHoursAndRate.Week2TimeOffHours
            }
        };

        PreTaxDeductionRequest preTaxDeductionRequest = new()
        {
            PreTaxDeduction = payInformationResponse.PreTaxDeduction,
            WeeklyHoursAndRate = weeklyHoursAndRate
        };

        PostTaxDeductionRequest postTaxDeductionRequest = new()
        {
            PostTaxDeduction = payInformationResponse.PostTaxDeduction
        };

        List<PayCheckRequest> payCheckRequest = new()
        {
            new PayCheckRequest()
            {
                TaxInformation = payInformationResponse.TaxInformation,
                EmployeeName = payInformationResponse.EmployeeName,
                PostTaxDeductionRequest = postTaxDeductionRequest,
                PreTaxDeductionRequest = preTaxDeductionRequest
            }
        };
        List<PayCheckResponse> payCheckResponse = await _payCheckService.CalculatePayCheck(payCheckRequest);

        IncomeInformationRequest incomeInformationRequest = new()
        {
            EmployeeName = payInformationResponse.EmployeeName,
            IncomeInformation = payInformationResponse,
            PayInformationId = payInformationResponse.PayInformationId,
            UserId = payInformationResponse.UserId
        }

        string incomeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:UpdatePayInformation").Value}/{userId}/{payId}";
        var incomeInformationResponse = await financeServiceClient.Post<>()< IncomeInformationRequest, IncomeInformationResponse>(incomeInformationRequest, incomeServiceUrl);
        return incomeInformationResponse;


        return payInformationResponse;
    }

    public async Task<bool> DeletePay(string userId, int payId)
    {
        HttpClient financeServiceClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:FinanceServiceClient:ClientName").Value);
        string financeServiceUrl = $"{_config.GetSection("Clients:FinanceServiceClient:DeletePayInformation").Value}/{userId}/{payId}";
        bool response = await financeServiceClient.Delete(financeServiceUrl);
        return response;
    }
}