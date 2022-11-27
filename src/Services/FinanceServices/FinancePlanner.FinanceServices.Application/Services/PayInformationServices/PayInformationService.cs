using AutoMapper;
using FinancePlanner.FinanceServices.Application.Features.PayInformation.Commands;
using FinancePlanner.FinanceServices.Application.Features.PayInformation.Queries;
using FinancePlanner.FinanceServices.Domain.Entities;
using FinancePlanner.FinanceServices.Infrastructure.Repositories;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.FinanceServices;

namespace FinancePlanner.FinanceServices.Application.Services.PayInformationServices;

public class PayInformationService : IPayInformationService
{
    private readonly IPayInformationRepository _payInformationRepository;
    private readonly IMapper _mapper;

    public PayInformationService(IPayInformationRepository payInformationRepository, IMapper mapper)
    {
        _payInformationRepository = payInformationRepository;
        _mapper = mapper;
    }

    public async Task<SavePayInformationCommandResponse> Save(PayInformationRequest request, string? userId, int? payId)
    {
        SavePayInformationCommandResponse response = new();
        if (userId != null && payId != null)
        {
            PayInformation? payInformationToUpdate = await _payInformationRepository.GetOne(userId, (int)payId);
            if (payInformationToUpdate == null)
            {
                throw new NotFoundException($"Record not found for user id: {userId} and pay id: {payId}");
            }
            PayInformation newPayInformation = _mapper.Map<PayInformation>(request);
            newPayInformation.BiWeeklyHoursAndRate.BiWeeklyHoursAndRateId = payInformationToUpdate.BiWeeklyHoursAndRateId;
            newPayInformation.PostTaxDeduction.PostTaxDeductionId = payInformationToUpdate.PostTaxDeductionId;
            newPayInformation.PreTaxDeduction.PreTaxDeductionId = payInformationToUpdate.PreTaxDeductionId;
            newPayInformation.TaxInformation.TaxInformationId = payInformationToUpdate.TaxInformationId;

            payInformationToUpdate.EmployeeName = newPayInformation.EmployeeName;
            payInformationToUpdate.BiWeeklyHoursAndRate = newPayInformation.BiWeeklyHoursAndRate;
            payInformationToUpdate.PostTaxDeduction = newPayInformation.PostTaxDeduction;
            payInformationToUpdate.PreTaxDeduction = newPayInformation.PreTaxDeduction;
            payInformationToUpdate.TaxInformation = newPayInformation.TaxInformation;
            bool updateResponse = await _payInformationRepository.Update(payInformationToUpdate);
            if (!updateResponse)
            {
                throw new NotUpdatedException($"Record not updated for user id: {userId} and pay id: {payId}");
            }
            response.PayInformationResponse = _mapper.Map<PayInformationResponse>(payInformationToUpdate);
            return response;
        }
        PayInformation payInformation = _mapper.Map<PayInformation>(request);
        payInformation.UserId = "test123";
        bool saveResponse = await _payInformationRepository.Save(payInformation);
        if (!saveResponse)
        {
            throw new NotUpdatedException($"Record not updated for user id: {userId} and pay id: {payId}");
        }
        response.PayInformationResponse = _mapper.Map<PayInformationResponse>(payInformation);
        return response;
    }

    public async Task<GetPayInformationQueryResponse> Get(string userId, int? payId)
    {
        List<PayInformationResponse> payInformationResponses = new();

        if (payId != null)
        {
            PayInformation? payInformation = await _payInformationRepository.GetOne(userId, (int)payId);
            if (payInformation == null) return new GetPayInformationQueryResponse();
            PayInformationResponse payInformationResponse = _mapper.Map<PayInformationResponse>(payInformation);
            payInformationResponses.Add(payInformationResponse);
            return new GetPayInformationQueryResponse { PayInformation = payInformationResponses };
        }

        List<PayInformation>? payInformationList = await _payInformationRepository.GetAll(userId);
        if (payInformationList == null) return new GetPayInformationQueryResponse();
        foreach (PayInformation payInformation in payInformationList)
        {
            PayInformationResponse payInformationResponse = _mapper.Map<PayInformationResponse>(payInformation);
            payInformationResponses.Add(payInformationResponse);
        }
        return new GetPayInformationQueryResponse { PayInformation = payInformationResponses };
    }

    public async Task<DeletePayInformationCommandResponse> Delete(string userId, int payId)
    {
        bool response = await _payInformationRepository.DeleteOne(userId, payId);
        return new DeletePayInformationCommandResponse { IsSuccessful = response };
    }
}