using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
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

        public async Task<SearchLearnerDetailsListViewModel> SearchLearnerDetailsAsync(long providerUkprn, int academicYear)
        {
            var apiRequest = new SearchLearnerRequest { Ukprn = providerUkprn, AcademicYear = new List<int> { academicYear } };
            var apiResponse = await _internalApiClient.SearchLearnerDetailsAsync(apiRequest);

            return _mapper.Map<SearchLearnerDetailsListViewModel>(apiResponse);
        }

        public async Task<SearchLearnerFiltersViewModel> GetSearchLearnerFiltersAsync(long providerUkprn)
        {
            var apiResponse = await _internalApiClient.GetSearchLearnerFiltersAsync(providerUkprn);
            return _mapper.Map<SearchLearnerFiltersViewModel>(apiResponse);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _internalApiClient.FindLearnerRecordAsync(providerUkprn, uln);
        }

        public async Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
            return _mapper.Map<T>(response);
        }
        
        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddMathsStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }
        
        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddEnglishStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }
    }
}