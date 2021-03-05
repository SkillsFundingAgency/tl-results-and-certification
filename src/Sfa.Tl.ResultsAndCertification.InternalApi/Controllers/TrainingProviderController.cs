using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
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
        [Route("FindProvidersUln/{providerUkprn}/{uln}")]
        public async Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderService.FindProvidersUlnAsync(providerUkprn, uln);
        }
    }
}
