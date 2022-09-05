namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld
{
    public class FedTaxWithheldResponse
    {
        public decimal TaxableWage { get; set; }
        public decimal FederalTaxWithheldAmount { get; set; }
    }
}
