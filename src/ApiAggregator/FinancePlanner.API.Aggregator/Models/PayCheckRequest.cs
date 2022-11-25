using System.Collections.Generic;
using FinancePlanner.Shared.Models.Enums;
using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.API.Aggregator.Models
{
    public class PayCheckRequest
    {
        public List<Company> Companies { get; set; }

        public PayCheckRequest()
        {
            Companies = new List<Company>();
        }
    }

    public class Company
    {
        public PreTaxWagesRequest PreTaxWagesRequest { get; set; }
        public string W4Type { get; set; }
        public TaxFilingStatus TaxFilingStatus { get; set; }
        public int PayPeriodNumber { get; set; }
        public string State { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public PostTaxDeductionRequest PostTaxDeductionRequest { get; set; }
    }
}
