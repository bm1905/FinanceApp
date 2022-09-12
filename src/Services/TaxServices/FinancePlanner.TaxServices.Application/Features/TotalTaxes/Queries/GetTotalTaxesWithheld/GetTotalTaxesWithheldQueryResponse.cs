namespace FinancePlanner.TaxServices.Application.Features.TotalTaxes.Queries.GetTotalTaxesWithheld
{
    public class GetTotalTaxesWithheldQueryResponse
    {
        public decimal FederalTaxableWage { get; set; }
        public decimal MedicareTaxableWage { get; set; }
        public decimal SocialSecurityTaxableWage { get; set; }
        public decimal FederalTaxWithheldAmount { get; set; }
        public decimal MedicareWithheldAmount { get; set; }
        public decimal SocialSecurityWithheldAmount { get; set; }
        public decimal TotalTaxesWithheldAmount { get; set; }
    }
}
