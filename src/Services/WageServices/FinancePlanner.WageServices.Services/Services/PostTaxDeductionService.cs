using System;
using System.Collections.Generic;
using Shared.Models.Exceptions;
using Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services
{
    public class PostTaxDeductionService : IPostTaxDeductionService
    {
        public PostTaxDeductionResponse CalculatePostTaxDeductions(PostTaxDeductionRequest request)
        {
            try
            {
                Dictionary<string, decimal> otherDeductionResponse = new Dictionary<string, decimal>();
                decimal totalDeductions = request.EmployeeStockPlan + request.Roth401KPercentage/100 * request.TotalGrossPay;
                foreach (KeyValuePair<string, decimal> otherDeductionRequest in request.OtherDeductions)
                {
                    otherDeductionResponse.Add(otherDeductionRequest.Key, otherDeductionRequest.Value);
                    totalDeductions += otherDeductionRequest.Value;
                }

                return new PostTaxDeductionResponse()
                {
                    TotalGrossPay = request.TotalGrossPay,
                    EmployeeStockPlan = request.EmployeeStockPlan,
                    Roth401KPercentage = request.Roth401KPercentage,
                    OtherDeductions = otherDeductionResponse,
                    TotalPostTaxDeductions = totalDeductions
                };
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message, ex);
            }
        }
    }
}
