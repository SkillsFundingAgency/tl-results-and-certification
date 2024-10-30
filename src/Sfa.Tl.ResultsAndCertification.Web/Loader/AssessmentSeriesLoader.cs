using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AssessmentSeriesLoader : IAssessmentSeriesLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly ILogger<AssessmentSeriesLoader> _logger;

        public AssessmentSeriesLoader(ILogger<AssessmentSeriesLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _internalApiClient = internalApiClient; ;
        }

        public async Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync()
            => await _internalApiClient.GetResultCalculationAssessmentAsync();
    }
}
