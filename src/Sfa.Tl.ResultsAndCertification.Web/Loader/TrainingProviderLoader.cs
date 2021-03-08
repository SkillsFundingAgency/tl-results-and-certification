using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class TrainingProviderLoader : ITrainingProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public TrainingProviderLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<bool> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _internalApiClient.FindLearnerRecordAsync(providerUkprn, uln);
        }
    }
}
