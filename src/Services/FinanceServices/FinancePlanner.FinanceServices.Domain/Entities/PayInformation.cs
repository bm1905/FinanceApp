using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancePlanner.FinanceServices.Domain.Entities;

public class PayInformation
{
    [Key] 
    public int PayInformationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public int BiWeeklyHoursAndRateId { get; set; }
    [ForeignKey("BiWeeklyHoursAndRateId")] 
    public virtual BiWeeklyHoursAndRate BiWeeklyHoursAndRate { get; set; } = new();
    public int PostTaxDeductionId { get; set; }
    [ForeignKey("PostTaxDeductionId")]
    public virtual PostTaxDeduction PostTaxDeduction { get; set; } = new();
    public int PreTaxDeductionId { get; set; }
    [ForeignKey("PreTaxDeductionId")]
    public virtual PreTaxDeduction PreTaxDeduction { get; set; } = new();
    public int TaxInformationId { get; set; }
    [ForeignKey("TaxInformationId")]
    public virtual TaxInformation TaxInformation { get; set; } = new();
}