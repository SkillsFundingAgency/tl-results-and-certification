using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction;
using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class ProviderAddressExtractionService : IProviderAddressExtractionService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderAddressExtractionService> _logger;

        public ProviderAddressExtractionService(IProviderRepository providerRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<ProviderAddressExtractionService> logger)
        {
            _providerRepository = providerRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessProviderAddressExtractionAsync(int[] academicYears)
        {
            IList<ProviderAddressDetails>  extractionData = await GetProviderAddressExtractionAsync(academicYears);

            if (extractionData == null || extractionData.Count == 0)
            {
                string message = $"No entries are found. Method: {nameof(ProcessProviderAddressExtractionAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new FunctionResponse { IsSuccess = true, Message = message };
            }

            var byteData = await CsvExtensions.WriteFileAsync(extractionData, classMapType: typeof(AnalystCoreResultExtractionDataExportMap));

            if (byteData.Length <= 0)
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessProviderAddressExtractionAsync)}()";
                throw new ApplicationException(message);
            }

            var blobUniqueReference = Guid.NewGuid();

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }
        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.ProviderAddressess.ToString(),
                SourceFilePath = Constants.ProviderAddressExtract,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
        private async Task<IList<ProviderAddressDetails>> GetProviderAddressExtractionAsync(int[] academicYears)
        {
            return await _providerRepository.GetProviderAddressesForRegistrations(academicYears);
           
        }
    }
}
