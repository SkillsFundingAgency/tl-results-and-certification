using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Linq;
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

            if (typeof(T) == typeof(AppealUpdatePathwayGradeViewModel))
            {
                var grades = await _internalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
                return _mapper.Map<T>(prsLearnerDetails, opt => opt.Items["grades"] = grades);
            }
            else
            return _mapper.Map<T>(prsLearnerDetails);
        }

        public async Task<bool> AppealCoreGradeAsync(long aoUkprn, AppealCoreGradeViewModel model)
        {
            var request = _mapper.Map<AppealGradeRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AppealGradeAsync(request);
        }
    }
}