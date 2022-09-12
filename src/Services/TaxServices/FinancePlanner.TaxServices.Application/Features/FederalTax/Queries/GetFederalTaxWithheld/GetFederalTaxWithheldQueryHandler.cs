using System;
using System.Threading;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Services.FederalTaxServices;
using FinancePlanner.TaxServices.Application.Services.FederalTaxServices.PluginHandler;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFederalTaxWithheld
{
    public class GetFederalTaxWithheldQueryHandler : IRequestHandler<GetFederalTaxWithheldQuery, GetFederalTaxWithheldQueryResponse>
    {
        private readonly FederalTaxPluginFactory _pluginFactory;

        public GetFederalTaxWithheldQueryHandler(FederalTaxPluginFactory pluginFactory)
        {
            _pluginFactory = pluginFactory;
        }

        public async Task<GetFederalTaxWithheldQueryResponse> Handle(GetFederalTaxWithheldQuery request, CancellationToken cancellationToken)
        {
            if (request.RequestModel.W4Type == null)
            {
                throw new ArgumentException();
            }

            IFederalTaxServices service = _pluginFactory.GetService<IFederalTaxServices>(request.RequestModel.W4Type);
            if (service == null)
            {
                throw new ApplicationException("Something went wrong while loading plugin!");
            }

            GetFederalTaxWithheldQueryResponse response = await service.CalculateFederalTaxWithheldAmount(request.RequestModel);
            return response;
        }
    }
}
