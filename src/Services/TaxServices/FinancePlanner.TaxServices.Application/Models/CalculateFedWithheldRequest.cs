using System.Collections.Generic;

namespace FinancePlanner.TaxServices.Application.Models
{
    public class CalculateFedWithheldRequest : BaseModel
    {
        public CalculateFedWithheldRequest(Dictionary<string, string> data)
        {
            Data = data;
        }

        public Dictionary<string, string> Data { get; set; }
    }
}
