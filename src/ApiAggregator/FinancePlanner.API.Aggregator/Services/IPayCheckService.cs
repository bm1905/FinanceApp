using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Models;

namespace FinancePlanner.API.Aggregator.Services
{
    public interface IPayCheckService
    {
        Task<PayCheckResponse> CalculatePayCheck(PayCheckRequest request);
    }
}
