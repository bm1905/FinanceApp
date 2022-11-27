using System.Collections.Generic;
using System.Threading.Tasks;
using FinancePlanner.API.Aggregator.Models;

namespace FinancePlanner.API.Aggregator.Services;

public interface IPayCheckService
{
    Task<List<PayCheckResponse>> CalculatePayCheck(List<PayCheckRequest> request);
}