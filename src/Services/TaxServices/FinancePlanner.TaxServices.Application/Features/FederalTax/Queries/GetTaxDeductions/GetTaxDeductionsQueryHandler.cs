using System;
using System.Threading;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Contracts;
using FinancePlanner.TaxServices.Application.PluginHandler;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetTaxDeductions
{
    public class GetTaxDeductionsQueryHandler : IRequestHandler<GetTaxDeductionsQuery, TaxDeductionsResponse>
    {
        private readonly PluginFactory _pluginFactory;

        public GetTaxDeductionsQueryHandler(PluginFactory pluginFactory)
        {
            _pluginFactory = pluginFactory;
        }

        public async Task<TaxDeductionsResponse> Handle(GetTaxDeductionsQuery request, CancellationToken cancellationToken)
        {
            if (request.RequestModel.W4Type == null)
            {
                throw new ArgumentException();
            }

            IFederalTaxWithheldCalculator service = _pluginFactory.GetService<IFederalTaxWithheldCalculator>(request.RequestModel.W4Type);
            if (service == null)
            {
                throw new ApplicationException("Something went wrong while loading plugin!");
            }

            TaxDeductionsResponse response = await service.CalculateTaxDeductions(request.RequestModel);
            return response;
        }
    }
}
