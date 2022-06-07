using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProviderController : ControllerBase, ITrainingProviderController
    {
        protected ITrainingProviderService _trainingProviderService;

        public TrainingProviderController(ITrainingProviderService trainingProviderService)
        {
            _trainingProviderService = trainingProviderService;
        }

        [HttpGet]
        [Route("FindLearnerRecord/{providerUkprn}/{uln}")]
        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderService.FindLearnerRecordAsync(providerUkprn, uln);
        }

        [HttpGet]
        [Route("GetLearnerRecordDetails/{providerUkprn}/{profileId}/{pathwayId:int?}")]
        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            return await _trainingProviderService.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }
        
        [HttpPut]
        [Route("UpdateLearnerSubject")]
        public async Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request)
        {
            return await _trainingProviderService.UpdateLearnerSubjectAsync(request);
        }

        [HttpPost]
        [Route("SearchLearnerDetails")]
        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            return await _trainingProviderService.SearchLearnerDetailsAsync(request);
        }

        [HttpGet]
        [Route("GetSearchLearnerFilters/{providerukprn}")]
        public async Task<SearchLearnerFilters> GetSearchLearnerFiltersAsync(long providerUkprn)
        {
            return await _trainingProviderService.GetSearchLearnerFiltersAsync(providerUkprn);
        }
    }
}