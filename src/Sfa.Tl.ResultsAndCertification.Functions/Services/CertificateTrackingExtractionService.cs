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
using Sfa.Tl.ResultsAndCertification.Models.CertificateTrackingExtraction;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class CertificateTrackingExtractionService : ICertificateTrackingExtractionService
    {
        private readonly ICertificateRepository _repository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ICertificateTrackingExtractionService> _logger;

        public CertificateTrackingExtractionService(
            ICertificateRepository repository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<ICertificateTrackingExtractionService> logger)
        {
            _repository = repository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FunctionResponse> ProcessCertificateTrackingExtractAsync(Func<DateTime> getFromDay, Func<string> getExtractFileName)
        {
            IList<CertificateTrackingExtractionData> data = await GetCertificateTrackingExtractionData(getFromDay);
            var byteData = await CsvExtensions.WriteFileAsync(data, classMapType: typeof(CertificateTrackingExtractionDataExportMap));

            if (byteData.IsNullOrEmpty())
            {
                var message = $"No byte data available to write. Method: {nameof(ProcessCertificateTrackingExtractAsync)}()";
                throw new ApplicationException(message);
            }

            BlobStorageData blobStorageData = CreateBlobStorageData(byteData, getExtractFileName);
            await _blobStorageService.UploadFromByteArrayAsync(blobStorageData);

            return new FunctionResponse { IsSuccess = true };
        }

        private async Task<IList<CertificateTrackingExtractionData>> GetCertificateTrackingExtractionData(Func<DateTime> getFromDay)
        {
            IList<PrintCertificate> printCertificates = await _repository.GetCertificateTrackingDataAsync(getFromDay);
            return _mapper.Map<IList<CertificateTrackingExtractionData>>(printCertificates);
        }

        private static BlobStorageData CreateBlobStorageData(byte[] data, Func<string> getExtractFileName)
        {
            return new BlobStorageData
            {
                ContainerName = DocumentType.CertificateTracking.ToString(),
                SourceFilePath = Constants.CertificateTrackingExtractsFolder,
                BlobFileName = getExtractFileName(),
                FileData = data,
                UserName = Constants.FunctionPerformedBy
            };
        }
    }
}