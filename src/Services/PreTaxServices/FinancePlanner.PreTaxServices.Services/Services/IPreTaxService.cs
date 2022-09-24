using FinancePlanner.PreTaxServices.Services.Models.DTOs;

namespace FinancePlanner.PreTaxServices.Services.Services
{
    public interface IPreTaxService
    {
        PreTaxWagesResponse CalculateTaxableWages(PreTaxWagesRequest request);
    }
}
