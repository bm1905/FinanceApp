using System.Threading.Tasks;
using FinancePlanner.TaxServices.Domain.Entities;

namespace FinancePlanner.TaxServices.Infrastructure.Repositories
{
    public interface IFederalTaxBracketRepository
    {
        Task<PercentageMethodTable> GetFederalTaxPercentage(decimal adjustedAnnualWage, string tableName);
    }
}
