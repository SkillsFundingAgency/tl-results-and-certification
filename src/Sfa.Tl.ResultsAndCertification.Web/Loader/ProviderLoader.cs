using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<object>> GetAllProvidersByUkprnAsync(long ukprn)
        {
            return await _internalApiClient.GetAllProvidersByUkprnAsync(ukprn);
        }

        public async Task<IEnumerable<object>> SearchByTokenAsync(string term)
        {
            return await _internalApiClient.GetAllProvidersAsync(term, true);
        }
    }
}
