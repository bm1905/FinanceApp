namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions
{
    public class TaxDeductionsResponse
    {
        public decimal TaxableWage { get; set; }
        public decimal FederalTaxWithheldAmount { get; set; }
        public decimal StateTaxWithheldAmount { get; set; }
        public decimal MedicareWithheldAmount { get; set; }
        public decimal SocialSecurityWithheldAmount { get; set; }
    }
}
