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
        [Route("FindLearnerRecord/{providerUkprn}/{uln}")]
        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderService.FindLearnerRecordAsync(providerUkprn, uln);
        }

        [HttpGet]
        [Route("GetLearnerRecordDetails/{providerUkprn}/{profileId}")]
        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            return await _trainingProviderService.GetLearnerRecordDetailsAsync(providerUkprn, profileId);
        }

        [HttpPost]
        [Route("AddLearnerRecord")]
        public async Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request)
        {
            return await _trainingProviderService.AddLearnerRecordAsync(request);
        }
    }
}
