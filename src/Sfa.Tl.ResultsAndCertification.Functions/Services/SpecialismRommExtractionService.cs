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
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class SpecialismRommExtractionService : ISpecialismRommExtractionService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<SpecialismRommExtractionService> _logger;

        public SpecialismRommExtractionService(
            IRegistrationRepository registrationRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<SpecialismRommExtractionService> logger)
        {
            _registrationRepository = registrationRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessSpecialismRommExtractsAsync(string[] assesmentSeriesYears)
        {
            IList<SpecialRommExtractionData> extractionData = await GetSpecialismRommExtractionData(assesmentSeriesYears);

            if (extractionData == null || extractionData.Count == 0)
            {
                string message = $"No entries are found. Method: {nameof(ProcessSpecialismRommExtractsAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new FunctionResponse { IsSuccess = true, Message = message };
            }

            var byteData = await CsvExtensions.WriteFileAsync(extractionData, classMapType: typeof(SpecialRommExtractionDataExportMap));

            if (byteData.Length <= 0)
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessSpecialismRommExtractsAsync)}()";
                throw new ApplicationException(message);
            }

            var blobUniqueReference = Guid.NewGuid();

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }

        private async Task<IList<SpecialRommExtractionData>> GetSpecialismRommExtractionData(string[] assesmentSeriesYears)
        {
            IList<TqRegistrationSpecialism> registrationPathways = await _registrationRepository.GetSpecialismRegistrationPathwaysByAssesmentSeriesYear(assesmentSeriesYears);
            return _mapper.Map<IList<SpecialRommExtractionData>>(registrationPathways);
        }

        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.SpecialismRomm.ToString(),
                SourceFilePath = Constants.SpecialismRommFolder,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}
