namespace FinancePlanner.TaxServices.Domain.Entities
{
    public class W4FromOrAfter2020Model
    {
        // This is Line 4a for 2022 W4 Form
        public decimal OtherIncome { get; set; }
        // This is Line 4b for 2022 W4 Form
        public decimal Deductions { get; set; }
        // This is Line 4c for 2022 W4 Form
        public decimal ExtraWithholding { get; set; }
        // This is step 2 for 2022 W4 Form
        public bool IsMultipleJobsChecked { get; set; }
        // This is step 3 for 2022 W4 Form
        public decimal ClaimDependentsAmount { get; set; }
    }
}
