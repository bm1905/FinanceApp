namespace FinancePlanner.PreTaxServices.Services.Models.DTOs
{
    public class PreTaxWagesResponse
    {
        public double GrossPay { get; set; }
        public double TotalPreTaxDeductions { get; set; }
        public double StateAndFederalTaxableWages { get; set; }
        public double SocialAndMedicareTaxableWages { get; set; }
    }
}
