using System.Collections.Generic;
using FinancePlanner.Shared.Models.FinanceServices;
using System.Threading.Tasks;

namespace FinancePlanner.API.Aggregator.Services;

public interface IFinanceService
{
    Task<IncomeInformationResponse> SavePay(PayInformationRequest request, string? userId, int? payId, int? incomeId);
    Task<List<PayInformationResponse>> GetPayList(string userId, int? payId);
    Task<List<IncomeInformationResponse>> GetIncomeList(string userId, int? incomeId);
    Task<bool> DeletePay(string userId, int payId, int incomeId);
}