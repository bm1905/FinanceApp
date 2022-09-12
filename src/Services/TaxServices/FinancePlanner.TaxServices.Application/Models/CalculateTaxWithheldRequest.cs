using System.Collections.Generic;

namespace FinancePlanner.TaxServices.Application.Models
{
    public class CalculateTaxWithheldRequest : BaseModel
    {
        public Dictionary<string, string> Data { get; set; }
    }
}
