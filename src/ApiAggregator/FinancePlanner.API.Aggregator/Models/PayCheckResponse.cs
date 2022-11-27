using FinancePlanner.Shared.Models.TaxServices;
using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.API.Aggregator.Models;

public class PayCheckResponse
{
    public string EmployeeName { get; set; } = string.Empty;
    public TotalTaxesWithheldResponse TaxesWithheldResponse { get; set; } = new();
    public PreTaxDeductionResponse PreTaxDeductionResponse { get; set; } = new();
    public PostTaxDeductionResponse PostTaxDeductionResponse { get; set; } = new();
}