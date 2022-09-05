using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld;
using FinancePlanner.TaxServices.Application.Models;

namespace FinancePlanner.TaxServices.Application.Contracts
{
    public interface IFederalTaxWithheldCalculator
    {
        Task<FedTaxWithheldResponse> CalculateFederalTaxWithheldAmount(CalculateFedWithheldRequest w4Model);
    }
}
