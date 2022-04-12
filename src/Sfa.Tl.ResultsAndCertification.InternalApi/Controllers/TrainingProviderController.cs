using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
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
        [Route("FindLearnerRecord/{providerUkprn}/{uln}/{evaluateSendConfirmation:bool?}")]
        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln, bool? evaluateSendConfirmation)
        {
            // TODO: This endpoint will be removed soon after implementing the ProviderSearchResult Page.
            return await _trainingProviderService.FindLearnerRecordAsync(providerUkprn, uln, evaluateSendConfirmation);
        }

        [HttpGet]
        [Route("GetLearnerRecordDetails/{providerUkprn}/{profileId}/{pathwayId:int?}")]
        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            return await _trainingProviderService.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }

        [HttpPost]
        [Route("AddLearnerRecord")]
        public async Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request)
        {
            return await _trainingProviderService.AddLearnerRecordAsync(request);
        }

        [HttpPut]
        [Route("UpdateLearnerRecord")]
        public async Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest model)
        {
            return await _trainingProviderService.UpdateLearnerRecordAsync(model);
        }
    }
}