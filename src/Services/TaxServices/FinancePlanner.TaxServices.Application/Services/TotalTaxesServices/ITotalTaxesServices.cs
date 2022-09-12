using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.TotalTaxes.Queries.GetTotalTaxesWithheld;
using FinancePlanner.TaxServices.Application.Models;

namespace FinancePlanner.TaxServices.Application.Services.TotalTaxesServices
{
    public interface ITotalTaxesServices
    {
        Task<GetTotalTaxesWithheldQueryResponse> CalculateTotalTaxesWithheldAmount(CalculateTaxWithheldRequest request);
    }
}
