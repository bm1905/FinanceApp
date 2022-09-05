﻿using FinancePlanner.TaxServices.Application.Enums;

namespace TaxServices.Plugins.W4Before2020.Models
{
    public class W4Before2020Model
    {
        // This is Line 5 for 2019 W4 Form
        public int AllowanceNumber { get; set; }
        // This is Line 6 for 2019 W4 Form
        public decimal AdditionalAmountToWithheld { get; set; }
        public string W4Type { get; set; }
        public TaxFilingStatus TaxFilingStatus { get; set; }
        public decimal TaxableWage { get; set; }
        public int PayPeriodNumber { get; set; }
    }
}
