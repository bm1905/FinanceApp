namespace FinancePlanner.PreTaxServices.Services.Models.Entities
{
    public class PreTaxDeduction
    {
        public double Medical { get; set; }
        public double Dental { get; set; }
        public double Vision { get; set; }
        public double Traditional401KPercentage { get; set; }
        public double HealthSavingAccount { get; set; }
        public double Others { get; set; }
    }
}
