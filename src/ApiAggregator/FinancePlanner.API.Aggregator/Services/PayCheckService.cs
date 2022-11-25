using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Models;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.TaxServices;
using FinancePlanner.Shared.Models.WageServices;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.API.Aggregator.Services
{
    public class PayCheckService : IPayCheckService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public PayCheckService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<PayCheckResponse> CalculatePayCheck(PayCheckRequest request)
        {
            PayCheckResponse finalPayCheckResponse = new();
            foreach (Company requestCompany in request.Companies)
            {
                Response payCheckResponse = await CalculatePayForEachCompany(requestCompany, _config);
                finalPayCheckResponse.Responses.Add(payCheckResponse);
                finalPayCheckResponse.TotalGrossPay += payCheckResponse.GrossPay;
                finalPayCheckResponse.TotalNetPay += payCheckResponse.NetPay;
            }
            return finalPayCheckResponse;
        }

        private async Task<Response> CalculatePayForEachCompany(Company requestCompany, IConfiguration config)
        {
            // Pre-Tax
            PreTaxWagesResponse preTaxWagesResponse = await PostAsync<PreTaxWagesRequest, PreTaxWagesResponse>(requestCompany.PreTaxWagesRequest,
                config.GetSection("Clients:WageServiceClient:ClientName").Value,
                config.GetSection("Clients:WageServiceClient:CalculateTotalTaxableWages").Value);

            // Tax
            CalculateTaxWithheldRequest calculateTaxWithheldRequest = new()
            {
                Data = requestCompany.Data,
                W4Type = requestCompany.W4Type,
                TaxFilingStatus = requestCompany.TaxFilingStatus,
                PayPeriodNumber = requestCompany.PayPeriodNumber,
                State = requestCompany.State,
                FederalTaxableWage = preTaxWagesResponse.StateAndFederalTaxableWages,
                StateTaxableWage = preTaxWagesResponse.StateAndFederalTaxableWages,
                MedicareTaxableWage = preTaxWagesResponse.SocialAndMedicareTaxableWages,
                SocialSecurityTaxableWage = preTaxWagesResponse.SocialAndMedicareTaxableWages
            };
            TotalTaxesWithheldResponse totalTaxesWithheldResponse = await PostAsync<CalculateTaxWithheldRequest, TotalTaxesWithheldResponse>(calculateTaxWithheldRequest,
                config.GetSection("Clients:TaxServiceClient:ClientName").Value,
                config.GetSection("Clients:TaxServiceClient:CalculateTotalTaxesWithheld").Value);

            // Post-Tax
            PostTaxDeductionRequest postTaxDeductionRequest = new()
            {
                Roth401KPercentage = requestCompany.PostTaxDeductionRequest.Roth401KPercentage,
                EmployeeStockPlan = requestCompany.PostTaxDeductionRequest.EmployeeStockPlan,
                OtherDeductions = requestCompany.PostTaxDeductionRequest.OtherDeductions,
                TotalGrossPay = preTaxWagesResponse.GrossPay
            };
            PostTaxDeductionResponse postTaxDeductionResponse = await PostAsync<PostTaxDeductionRequest, PostTaxDeductionResponse>(postTaxDeductionRequest,
                config.GetSection("Clients:WageServiceClient:ClientName").Value,
                config.GetSection("Clients:WageServiceClient:CalculatePostTaxDeductions").Value);

            // Response
            Response response = new()
            {
                GrossPay = preTaxWagesResponse.GrossPay,
                NetPay = preTaxWagesResponse.GrossPay - preTaxWagesResponse.TotalPreTaxDeductions -
                         postTaxDeductionResponse.TotalPostTaxDeductions -
                         totalTaxesWithheldResponse.TotalTaxesWithheldAmount,
                PostTaxDeductions = postTaxDeductionResponse.TotalPostTaxDeductions,
                PreTaxDeductions = preTaxWagesResponse.TotalPreTaxDeductions,
                TotalTax = totalTaxesWithheldResponse.TotalTaxesWithheldAmount
            };
            return response;
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest model, string clientName, string url)
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            string requestJson = JsonSerializer.Serialize(model);
            StringContent requestContent = new(requestJson, Encoding.UTF8, "application/json");
            HttpClient client = _httpClientFactory.CreateClient(clientName);
            HttpResponseMessage response = await client.PostAsync(url, requestContent);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException($"Request: {response.StatusCode}", $"The request for client {clientName} and endpoint {url} is not authorized.");
            }

            if (!response.IsSuccessStatusCode)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(errorResponse)) throw new InternalServerErrorException($"API call error out with status {response.StatusCode}");
                ExceptionModel exceptionModel = JsonSerializer.Deserialize<ExceptionModel>(errorResponse, options);
                if (exceptionModel == null)
                {
                    throw new InternalServerErrorException($"API call error out with {response.StatusCode}");
                }
                throw new ApiErrorException(exceptionModel.Message ?? "API call error", exceptionModel.Details ?? string.Empty);
            }

            string responseString = await response.Content.ReadAsStringAsync();
            TResponse responseModel = JsonSerializer.Deserialize<TResponse>(responseString, options);
            if (responseModel == null)
            {
                throw new InternalServerErrorException($"{nameof(TResponse)} is empty or null");
            }

            return responseModel;
        }
    }
}
