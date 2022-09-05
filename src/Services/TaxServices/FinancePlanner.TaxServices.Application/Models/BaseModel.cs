using FinancePlanner.TaxServices.Application.Enums;

namespace FinancePlanner.TaxServices.Application.Models
{
    public class BaseModel
    {
        public string W4Type { get; set; }
        public TaxFilingStatus TaxFilingStatus { get; set; }
        public decimal TaxableWage { get; set; }
        public int PayPeriodNumber { get; set; }
        public string State { get; set; }
    }
}
