using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinancePlanner.PreTaxServices.Services.Models.Entities;

namespace FinancePlanner.PreTaxServices.Services.Models.DTOs
{
    public class PreTaxWagesRequest
    {
        [Required]
        public List<WeeklyHour> WeeklyHours { get; set; }
        [Required]
        public PreTaxDeduction PreTaxDeduction { get; set; }
    }
}
