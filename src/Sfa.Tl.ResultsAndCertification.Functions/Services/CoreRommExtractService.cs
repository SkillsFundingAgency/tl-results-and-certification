﻿using AutoMapper;
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
        private readonly ILogger<CoreRommExtractService> _logger;

        public CoreRommExtractService(
            IRegistrationRepository registrationRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<CoreRommExtractService> logger)
        {
            _registrationRepository = registrationRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessCoreRommExtractAsync(string[] assessmentSeriesYears)
        {
            IList<CoreRommExtractData> coreRommExtractData = await GetCoreRommExtractData(assessmentSeriesYears);

            if (coreRommExtractData.IsNullOrEmpty())
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

        private async Task<IList<CoreRommExtractData>> GetCoreRommExtractData(string[] assesmentSeriesYears)
        {
            IList<TqRegistrationPathway> registrationPathways = await _registrationRepository.GetRegistrationPathwaysByAssesmentSeriesYear(assesmentSeriesYears);
            return _mapper.Map<IList<CoreRommExtractData>>(registrationPathways);
        }

        private BlobStorageData CreateBlobStorageData(byte[] data)
        {
            Guid blobUniqueReference = Guid.NewGuid();

            return new BlobStorageData
            {
                ContainerName = DocumentType.CoreRomm.ToString(),
                SourceFilePath = Constants.CoreRommFolder,
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}