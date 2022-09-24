using System;
using FinancePlanner.PreTaxServices.Services.Exceptions;
using FinancePlanner.PreTaxServices.Services.Models.DTOs;
using FinancePlanner.PreTaxServices.Services.Models.Entities;

namespace FinancePlanner.PreTaxServices.Services.Services
{
    public class PreTaxService : IPreTaxService
    {
        public PreTaxWagesResponse CalculateTaxableWages(PreTaxWagesRequest request)
        {
            try
            {
                double totalGrossPay = 0;

                // Calculate total gross pay
                foreach (WeeklyHour weeklyHour in request.WeeklyHours)
                {
                    double rate = weeklyHour.HourlyRate;
                    double overTime = (weeklyHour.TotalHours - weeklyHour.TimeOffHours) > 40
                        ? (weeklyHour.TotalHours - weeklyHour.TimeOffHours - 40)
                        : 0;
                    double regularHours = weeklyHour.TotalHours - weeklyHour.TimeOffHours - overTime;
                    totalGrossPay += regularHours * rate + weeklyHour.TimeOffHours * rate + overTime * (rate + rate / 2);
                }

                double totalPreTaxDeductions = request.PreTaxDeduction.Dental +
                                               request.PreTaxDeduction.HealthSavingAccount +
                                               request.PreTaxDeduction.Medical +
                                               request.PreTaxDeduction.Vision +
                                               request.PreTaxDeduction.Others +
                                               request.PreTaxDeduction.Traditional401KPercentage / 100 * totalGrossPay;


                return new PreTaxWagesResponse()
                {
                    GrossPay = totalGrossPay,
                    TotalPreTaxDeductions = totalPreTaxDeductions,
                    SocialAndMedicareTaxableWages = totalGrossPay - totalPreTaxDeductions + request.PreTaxDeduction.Traditional401KPercentage / 100 * totalGrossPay,
                    StateAndFederalTaxableWages = totalGrossPay - totalPreTaxDeductions
                };
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message, ex);
            }
        }
    }
}
