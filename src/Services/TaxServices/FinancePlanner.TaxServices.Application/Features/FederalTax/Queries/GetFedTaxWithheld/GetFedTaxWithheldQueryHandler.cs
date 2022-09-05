using System;
using System.Threading;
using System.Threading.Tasks;
using FinancePlanner.TaxServices.Application.Contracts;
using FinancePlanner.TaxServices.Application.PluginHandler;
using MediatR;

namespace FinancePlanner.TaxServices.Application.Features.FederalTax.Queries.GetFedTaxWithheld
{
    public class GetFedTaxWithheldQueryHandler : IRequestHandler<GetFedTaxWithheldQuery, FedTaxWithheldResponse>
    {
        private readonly PluginFactory _pluginFactory;

        public GetFedTaxWithheldQueryHandler(PluginFactory pluginFactory)
        {
            _pluginFactory = pluginFactory;
        }

        public async Task<FedTaxWithheldResponse> Handle(GetFedTaxWithheldQuery request, CancellationToken cancellationToken)
        {
            if (request.RequestModel.W4Type == null)
            {
                throw new ArgumentException();
            }

            IFederalTaxWithheldCalculator? service = _pluginFactory.GetService<IFederalTaxWithheldCalculator>(request.RequestModel.W4Type);
            if (service == null)
            {
                throw new ApplicationException("Something went wrong while loading plugin!");
            }

            FedTaxWithheldResponse response = await service.CalculateFederalTaxWithheldAmount(request.RequestModel);
            return response;
        }
    }
}
