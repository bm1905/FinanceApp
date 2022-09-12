using FinancePlanner.TaxServices.Application.Enums;

namespace FinancePlanner.TaxServices.Application.Models
{
    public class BaseModel
    {
        public string W4Type { get; set; }
        public TaxFilingStatus TaxFilingStatus { get; set; }
        public decimal FederalTaxableWage { get; set; }
        public decimal StateTaxableWage { get; set; }
        public decimal MedicareTaxableWage { get; set; }
        public decimal SocialSecurityTaxableWage { get; set; }
        public int PayPeriodNumber { get; set; }
        public string State { get; set; }
    }
}
