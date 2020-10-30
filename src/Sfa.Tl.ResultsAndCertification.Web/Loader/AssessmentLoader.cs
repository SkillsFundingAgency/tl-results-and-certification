using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AssessmentLoader : IAssessmentLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssessmentLoader> _logger;

        public AssessmentLoader(IMapper mapper, ILogger<AssessmentLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _logger = logger;
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadAssessmentsResponseViewModel> ProcessBulkAssessmentsAsync(UploadAssessmentsRequestViewModel viewModel)
        {
            var bulkAssessmentRequest = _mapper.Map<BulkAssessmentRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkAssessmentRequest.DocumentType.ToString(),
                    BlobFileName = bulkAssessmentRequest.BlobFileName,
                    SourceFilePath = $"{bulkAssessmentRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkAssessmentRequest.PerformedBy
                });
            }

            var bulkAssessmentResponse = await _internalApiClient.ProcessBulkAssessmentsAsync(bulkAssessmentRequest);
            return _mapper.Map<UploadAssessmentsResponseViewModel>(bulkAssessmentResponse);
        }
    }
}
