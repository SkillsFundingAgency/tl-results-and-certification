using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class AnalystOverallResultExtractionService : IAnalystOverallResultExtractionService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AnalystOverallResultExtractionService> _logger;

        public AnalystOverallResultExtractionService(
            IRegistrationRepository registrationRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<AnalystOverallResultExtractionService> logger)
        {
            _registrationRepository = registrationRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessAnalystOverallResultExtractionData()
        {
            IList<AnalystOverallResultExtractionData> extractionData = await GetAnalystOverallResultExtractionData();

            if (extractionData.IsNullOrEmpty())
            {
                string message = $"No entries are found. Method: {nameof(ProcessAnalystOverallResultExtractionData)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new FunctionResponse { IsSuccess = false, Message = message };
            }

            var byteData = await CsvExtensions.WriteFileAsync(extractionData, classMapType: typeof(AnalystOverallResultExtractionDataExportMap));

            if (byteData.Length <= 0)
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessAnalystOverallResultExtractionData)}()";
                throw new ApplicationException(message);
            }

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }

        private async Task<IList<AnalystOverallResultExtractionData>> GetAnalystOverallResultExtractionData()
        {
            IList<TqRegistrationPathway> registrationPathways = await _registrationRepository.GetRegistrationPathways();
            return _mapper.Map<IList<AnalystOverallResultExtractionData>>(registrationPathways);
        }

        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.AnalystOverallResults.ToString(),
                SourceFilePath = Constants.AnalystOverallResultExtractsFolder,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}