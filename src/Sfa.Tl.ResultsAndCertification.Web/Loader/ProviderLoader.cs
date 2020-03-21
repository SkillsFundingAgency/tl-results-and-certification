using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
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

        public async Task<IEnumerable<string>> FindProviderNameAsync(string name, bool isExactMatch)
        {
            return await _internalApiClient.FindProviderNameAsync(name, isExactMatch);
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            return await _internalApiClient.IsAnyProviderSetupCompletedAsync(ukprn);
        }
    }
}
