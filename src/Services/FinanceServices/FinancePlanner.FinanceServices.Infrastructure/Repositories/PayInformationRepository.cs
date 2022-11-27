using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancePlanner.FinanceServices.Domain.Entities;
using FinancePlanner.FinanceServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancePlanner.FinanceServices.Infrastructure.Repositories;

public class PayInformationRepository : IPayInformationRepository
{
    private readonly FinanceDbContext _dbContext;

    public PayInformationRepository(FinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Save(PayInformation payInformation)
    {
        await _dbContext.AddAsync(payInformation);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(PayInformation payInformation)
    {
        _dbContext.Update(payInformation);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<PayInformation>?> GetAll(string userId)
    {
        if (_dbContext.BiWeeklyHoursAndRates == null 
            || _dbContext.PostTaxDeductions == null 
            || _dbContext.PreTaxDeductions == null 
            || _dbContext.TaxInformation == null) return default;

        List<PayInformation> payInformationList = await (from p in _dbContext.PayInformation
            join b in _dbContext.BiWeeklyHoursAndRates on p.BiWeeklyHoursAndRateId equals b.BiWeeklyHoursAndRateId
            join po in _dbContext.PostTaxDeductions on p.PostTaxDeductionId equals po.PostTaxDeductionId
            join pr in _dbContext.PreTaxDeductions on p.PreTaxDeductionId equals pr.PreTaxDeductionId
            join t in _dbContext.TaxInformation on p.TaxInformationId equals t.TaxInformationId
            where p.UserId == userId
            select new PayInformation
            {
                PayInformationId = p.PayInformationId,
                UserId = p.UserId,
                EmployeeName = p.EmployeeName,
                BiWeeklyHoursAndRateId = p.BiWeeklyHoursAndRateId,
                BiWeeklyHoursAndRate = b,
                PreTaxDeductionId = p.PreTaxDeductionId,
                PreTaxDeduction = pr,
                PostTaxDeductionId = p.PostTaxDeductionId,
                PostTaxDeduction = po,
                TaxInformationId = p.TaxInformationId,
                TaxInformation = t
            }
        ).AsNoTracking().ToListAsync();

        return payInformationList;
    }

    public async Task<PayInformation?> GetOne(string userId, int payId)
    {
        if (_dbContext.BiWeeklyHoursAndRates == null
            || _dbContext.PostTaxDeductions == null
            || _dbContext.PreTaxDeductions == null
            || _dbContext.TaxInformation == null) return default;

        PayInformation? payInformation = await (from p in _dbContext.PayInformation
                join b in _dbContext.BiWeeklyHoursAndRates on p.BiWeeklyHoursAndRateId equals b.BiWeeklyHoursAndRateId
                join po in _dbContext.PostTaxDeductions on p.PostTaxDeductionId equals po.PostTaxDeductionId
                join pr in _dbContext.PreTaxDeductions on p.PreTaxDeductionId equals pr.PreTaxDeductionId
                join t in _dbContext.TaxInformation on p.TaxInformationId equals t.TaxInformationId
                where p.UserId == userId
                where p.PayInformationId == payId
                select new PayInformation
                {
                    PayInformationId = p.PayInformationId,
                    UserId = p.UserId,
                    EmployeeName = p.EmployeeName,
                    BiWeeklyHoursAndRateId = p.BiWeeklyHoursAndRateId,
                    BiWeeklyHoursAndRate = b,
                    PreTaxDeductionId = p.PreTaxDeductionId,
                    PreTaxDeduction = pr,
                    PostTaxDeductionId = p.PostTaxDeductionId,
                    PostTaxDeduction = po,
                    TaxInformationId = p.TaxInformationId,
                    TaxInformation = t
                }
            ).AsNoTracking().SingleOrDefaultAsync();

        return payInformation;
    }

    public async Task<bool> DeleteOne(string userId, int payId)
    {
        if (_dbContext.PayInformation == null) return false;
        PayInformation? payInformation = await _dbContext.PayInformation.Where(x => x.UserId == userId && x.PayInformationId == payId)
            .Include(e => e.BiWeeklyHoursAndRate)
            .Include(e => e.PostTaxDeduction)
            .Include(e => e.PreTaxDeduction)
            .Include(e => e.TaxInformation)
            .FirstOrDefaultAsync();
        if (payInformation == null) return false;
        _dbContext.Remove(payInformation.BiWeeklyHoursAndRate);
        _dbContext.Remove(payInformation.PostTaxDeduction);
        _dbContext.Remove(payInformation.PreTaxDeduction);
        _dbContext.Remove(payInformation.TaxInformation);
        _dbContext.Remove(payInformation);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}