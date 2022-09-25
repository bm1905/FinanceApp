using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Models;
using Shared.Models.Exceptions;
using Shared.Models.TaxServices;
using Shared.Models.WageServices;

namespace FinancePlanner.API.Aggregator.Services
{
    public class PayCheckService : IPayCheckService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PayCheckService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PayCheckResponse> CalculatePayCheck(PayCheckRequest request)
        {
            PayCheckResponse finalPayCheckResponse = new PayCheckResponse();
            foreach (Company requestCompany in request.Companies)
            {
                PayCheckResponse payCheckResponse = await CalculatePayForEachCompany(requestCompany);
                finalPayCheckResponse.GrossPay += payCheckResponse.GrossPay;
                finalPayCheckResponse.NetPay += payCheckResponse.NetPay;
                finalPayCheckResponse.PostTaxDeductions += payCheckResponse.PostTaxDeductions;
                finalPayCheckResponse.PreTaxDeductions += payCheckResponse.PreTaxDeductions;
                finalPayCheckResponse.TotalTax += payCheckResponse.TotalTax;
            }
            return finalPayCheckResponse;
        }

        private async Task<PayCheckResponse> CalculatePayForEachCompany(Company requestCompany)
        {
            CalculateTaxWithheldRequest calculateTaxWithheldRequest = new CalculateTaxWithheldRequest();
            PostTaxDeductionRequest postTaxDeductionRequest = new PostTaxDeductionRequest();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            calculateTaxWithheldRequest.Data = requestCompany.Data;
            calculateTaxWithheldRequest.W4Type = requestCompany.W4Type;
            calculateTaxWithheldRequest.TaxFilingStatus = requestCompany.TaxFilingStatus;
            calculateTaxWithheldRequest.PayPeriodNumber = requestCompany.PayPeriodNumber;
            calculateTaxWithheldRequest.State = requestCompany.State;
            postTaxDeductionRequest.Roth401KPercentage = requestCompany.PostTaxDeductionRequest.Roth401KPercentage;
            postTaxDeductionRequest.EmployeeStockPlan = requestCompany.PostTaxDeductionRequest.EmployeeStockPlan;
            postTaxDeductionRequest.OtherDeductions = requestCompany.PostTaxDeductionRequest.OtherDeductions;

            string preTaxWagesRequestJson = JsonSerializer.Serialize(requestCompany.PreTaxWagesRequest);
            StringContent preTaxWagesRequestContent = new StringContent(preTaxWagesRequestJson, Encoding.UTF8, "application/json");

            HttpClient wageServicesClient = _httpClientFactory.CreateClient("WageServices");
            HttpResponseMessage wageServicesResponse = await wageServicesClient.PostAsync("/api/v1/Wage/CalculateTotalTaxableWages", preTaxWagesRequestContent);
            if (!wageServicesResponse.IsSuccessStatusCode)
            {
                throw new InternalServerErrorException($"WageServices API call failed with code {wageServicesResponse.StatusCode}");
            }

            string wageServicesResponseString = await wageServicesResponse.Content.ReadAsStringAsync();
            PreTaxWagesResponse preTaxWagesResponse = JsonSerializer.Deserialize<PreTaxWagesResponse>(wageServicesResponseString, options);

            if (preTaxWagesResponse == null)
            {
                throw new InternalServerErrorException("PreTaxWagesResponse is empty or null");
            }

            calculateTaxWithheldRequest.FederalTaxableWage = preTaxWagesResponse.StateAndFederalTaxableWages;
            calculateTaxWithheldRequest.StateTaxableWage = preTaxWagesResponse.StateAndFederalTaxableWages;
            calculateTaxWithheldRequest.MedicareTaxableWage = preTaxWagesResponse.SocialAndMedicareTaxableWages;
            calculateTaxWithheldRequest.SocialSecurityTaxableWage = preTaxWagesResponse.SocialAndMedicareTaxableWages;

            string calculateTaxWithheldRequestJson = JsonSerializer.Serialize(calculateTaxWithheldRequest);
            StringContent calculateTaxWithheldRequestContent = new StringContent(calculateTaxWithheldRequestJson, Encoding.UTF8, "application/json");

            HttpClient taxServicesClient = _httpClientFactory.CreateClient("TaxServices");
            HttpResponseMessage taxServicesResponse = await taxServicesClient.PostAsync("/api/v1/Tax/CalculateTotalTaxesWithheld", calculateTaxWithheldRequestContent);
            if (!taxServicesResponse.IsSuccessStatusCode)
            {
                throw new InternalServerErrorException($"TaxServices API call failed with code {taxServicesResponse.StatusCode}");
            }

            string taxServicesResponseString = await taxServicesResponse.Content.ReadAsStringAsync();
            TotalTaxesWithheldResponse totalTaxesWithheldResponse = JsonSerializer.Deserialize<TotalTaxesWithheldResponse>(taxServicesResponseString, options);

            if (totalTaxesWithheldResponse == null)
            {
                throw new InternalServerErrorException("TotalTaxesWithheldResponse is empty or null");
            }

            postTaxDeductionRequest.TotalGrossPay = preTaxWagesResponse.GrossPay;

            string postTaxDeductionRequestJson = JsonSerializer.Serialize(postTaxDeductionRequest);
            StringContent postTaxDeductionRequestContent = new StringContent(postTaxDeductionRequestJson, Encoding.UTF8, "application/json");

            HttpResponseMessage postTaxServicesResponse = await wageServicesClient.PostAsync("/api/v1/Wage/CalculatePostTaxDeductions", postTaxDeductionRequestContent);
            if (!postTaxServicesResponse.IsSuccessStatusCode)
            {
                throw new InternalServerErrorException($"WageServices API call failed with code {postTaxServicesResponse.StatusCode}");
            }

            string postTaxServicesResponseString = await postTaxServicesResponse.Content.ReadAsStringAsync();
            PostTaxDeductionResponse postTaxDeductionResponse = JsonSerializer.Deserialize<PostTaxDeductionResponse>(postTaxServicesResponseString, options);

            if (postTaxDeductionResponse == null)
            {
                throw new InternalServerErrorException("PostTaxDeductionResponse is empty or null");
            }

            PayCheckResponse response = new()
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
    }
}
