using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class CoreRommExtractService : ICoreRommExtractService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AnalystCoreResultExtractionService> _logger;

        public CoreRommExtractService(
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

        public async Task<FunctionResponse> ProcessCoreRommExtractAsync(int assesmentSeriesYear)
        {
            IList<CoreRommExtractData> coreRommExtractData = await GetCoreRommExtractData(assesmentSeriesYear);

            if (coreRommExtractData == null || coreRommExtractData.Count == 0)
            {
                string message = $"No entries are found. Method: {nameof(ProcessCoreRommExtractAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new FunctionResponse { IsSuccess = true, Message = message };
            }

            var byteData = await CsvExtensions.WriteFileAsync(coreRommExtractData, classMapType: typeof(CoreRommExtractDataExportMap));

            if (byteData.Length <= 0)
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessCoreRommExtractAsync)}()";
                throw new ApplicationException(message);
            }

            var blobUniqueReference = Guid.NewGuid();

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }

        private async Task<IList<CoreRommExtractData>> GetCoreRommExtractData(int assesmentSeriesYear)
        {
            IList<TqRegistrationPathway> registrationPathways = await _registrationRepository.GetRegistrationPathwaysByAssesmentSeriesYear(assesmentSeriesYear);
            return _mapper.Map<IList<CoreRommExtractData>>(registrationPathways);
        }

        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.CoreRommExtract.ToString(),
                SourceFilePath = Constants.CoreRommExtractFolder,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}