using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ProviderLoader : IProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public ProviderLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProviderLookupData>> GetProviderLookupDataAsync(string name, bool isExactMatch)
        {
            var providers = await _internalApiClient.FindProviderAsync(name, isExactMatch);
            return _mapper.Map<IEnumerable<ProviderLookupData>>(providers);
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            return await _internalApiClient.IsAnyProviderSetupCompletedAsync(ukprn);
        }

        public async Task<ProviderTlevelsViewModel> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var tlevelDetails = await _internalApiClient.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
            return _mapper.Map<ProviderTlevelsViewModel>(tlevelDetails);
        }
    }
}
