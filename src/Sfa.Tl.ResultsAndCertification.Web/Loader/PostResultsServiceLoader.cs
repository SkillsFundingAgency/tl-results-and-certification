using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class PostResultsServiceLoader : IPostResultsServiceLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public PostResultsServiceLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _internalApiClient.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        public async Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessementId)
        {
            var prsLearnerDetails = await _internalApiClient.GetPrsLearnerDetailsAsync(aoUkprn, profileId, assessementId);
            return _mapper.Map<T>(prsLearnerDetails);
        }

        public async Task<AppealCoreGradeViewModel> GetPrsAppealCoreGradeDetailsAsync(long aoUkprn, int profileId, int resultId)
        {
            var prsLearnerDetails = await _internalApiClient.GetPrsLearnerDetailsAsync(aoUkprn, profileId, 1); // TODO:
            return _mapper.Map<AppealCoreGradeViewModel>(prsLearnerDetails, opt => opt.Items["pathwayResultId"] = resultId);
        }
    }
}