using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class AnalystCoreResultExtractionService : IAnalystCoreResultExtractionService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AnalystCoreResultExtractionService> _logger;

        public AnalystCoreResultExtractionService(
            IRegistrationRepository registrationRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<AnalystCoreResultExtractionService> logger)
        {
            _registrationRepository = registrationRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessAnalystCoreResultExtractsAsync(int[] academicYears)
        {
            IList<AnalystCoreResultExtractionData> extractionData = await GetAnalystCoreResultExtractionData(academicYears);

            if (extractionData == null || extractionData.Count == 0)
            {
                string message = $"No entries are found. Method: {nameof(ProcessAnalystCoreResultExtractsAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new FunctionResponse { IsSuccess = true, Message = message };
            }

            var byteData = await CsvExtensions.WriteFileAsync(extractionData, classMapType: typeof(AnalystCoreResultExtractionDataExportMap));

            if (byteData.Length <= 0)
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessAnalystCoreResultExtractsAsync)}()";
                throw new ApplicationException(message);
            }

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }

        private async Task<IList<AnalystCoreResultExtractionData>> GetAnalystCoreResultExtractionData(int[] academicYears)
        {
            IList<TqRegistrationPathway> registrationPathways = await _registrationRepository.GetRegistrationPathwaysByAcademicYear(academicYears);
            return _mapper.Map<IList<AnalystCoreResultExtractionData>>(registrationPathways);
        }

        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.AnalystCoreResults.ToString(),
                SourceFilePath = Constants.AnalystCoreResultExtractsFolder,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}
