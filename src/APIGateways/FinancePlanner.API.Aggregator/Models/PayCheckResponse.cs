namespace FinancePlanner.API.Aggregator.Models
{
    public class PayCheckResponse
    {
        public decimal GrossPay { get; set; }
        public decimal NetPay { get; set; }
        public decimal PreTaxDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal PostTaxDeductions { get; set; }
    }
}
