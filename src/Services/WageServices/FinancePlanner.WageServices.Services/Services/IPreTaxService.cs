using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services
{
    public interface IPreTaxService
    {
        PreTaxWagesResponse CalculateTaxableWages(PreTaxWagesRequest request);
    }
}
