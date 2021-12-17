using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class DataExportLoader : IDataExportLoader
    {
        private readonly IDataExportService _dataExportService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DataExportLoader> _logger;


        public DataExportLoader(IDataExportService dataExportService)
        {
            _dataExportService = dataExportService;
        }

        public async Task<DataExportResponse> ProcessDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            var registrations = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn);
            if (registrations == null || !registrations.Any())
                return new DataExportResponse { IsDataFound = false };

            // 2. Write to CSV
            var byteData = await CsvExtensions.WriteFileAsync(registrations);

            // 3. Save to Blob
            var blobUniqueReference = Guid.NewGuid(); 
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString().ToLower(),
                SourceFilePath = $"{aoUkprn}/{requestType}",
                BlobFileName = blobUniqueReference.ToString(),
                FileData = byteData,
                UserName = requestedBy
            });

            // 4. Return response
            return new DataExportResponse
            {
                FileSize = Math.Round((byteData.Length / 1024D), 2),
                BlobUniqueReference = blobUniqueReference,
                IsDataFound = true
            };
        }
    }
}