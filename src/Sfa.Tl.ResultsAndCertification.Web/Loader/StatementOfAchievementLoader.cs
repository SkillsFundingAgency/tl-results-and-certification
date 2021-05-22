using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class StatementOfAchievementLoader : IStatementOfAchievementLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public StatementOfAchievementLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<T> FindSoaLearnerRecordAsync<T>(long providerUkprn, long uln)
        {
            var response = await _internalApiClient.FindSoaLearnerRecordAsync(providerUkprn, uln);
            return _mapper.Map<T>(response);
        }
    }
}