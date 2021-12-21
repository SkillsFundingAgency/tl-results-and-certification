using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class DataExportLoader : IDataExportLoader
    {
        private readonly IDataExportService _dataExportService;
        private readonly IBlobStorageService _blobStorageService;
        
        public DataExportLoader(IDataExportService dataExportService, IBlobStorageService blobStorageService)
        {
            _dataExportService = dataExportService;
            _blobStorageService = blobStorageService;
        }

        public async Task<IList<DataExportResponse>> ProcessDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            var response = new List<DataExportResponse>();
            byte[] byteData;
            DataExportResponse blobResponse;
            
            switch (requestType)
            {
                case DataExportType.Registrations:
                    var registrations = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn);
                    if (registrations == null || !registrations.Any())
                        return new List<DataExportResponse> { new DataExportResponse { IsDataFound = false } };

                    byteData = await CsvExtensions.WriteFileAsync(registrations, classMapType: typeof(RegistrationsExportMap));
                    blobResponse = await WriteCsvToBlobAsync(aoUkprn, requestType, requestedBy, byteData);
                    response.Add(blobResponse);
                    break;

                case DataExportType.Assessments:
                    // CoreAssessments
                    var coreAssessment = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn); // CoreAssessments
                    if (coreAssessment == null || !coreAssessment.Any())
                        return new List<DataExportResponse> { new DataExportResponse { IsDataFound = false } };

                    byteData = await CsvExtensions.WriteFileAsync(coreAssessment);
                    blobResponse = await WriteCsvToBlobAsync(aoUkprn, requestType, requestedBy, byteData);
                    response.Add(blobResponse);

                    // SpecialismAssessments
                    var specialismsAssessment = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn); // SpecialismsAssessment
                    if (specialismsAssessment == null || !specialismsAssessment.Any())
                        return new List<DataExportResponse> { new DataExportResponse { IsDataFound = false } };

                    byteData = await CsvExtensions.WriteFileAsync(coreAssessment);
                    blobResponse = await WriteCsvToBlobAsync(aoUkprn, requestType, requestedBy, byteData);
                    response.Add(blobResponse);

                    break;

                case DataExportType.Results:
                    break;

                default:
                    break;
            }

            return response;
        }

        private async Task<DataExportResponse> WriteCsvToBlobAsync(long aoUkprn, DataExportType requestType, string requestedBy, byte[] byteData)
        {
            // 3. Save to Blob
            var blobUniqueReference = Guid.NewGuid();
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString().ToLower(),
                SourceFilePath = $"{aoUkprn}/{requestType}",
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = byteData,
                UserName = requestedBy
            });

            // 4. Return response
            return new DataExportResponse
            {
                FileSize = byteData.Length,
                BlobUniqueReference = blobUniqueReference,
                IsDataFound = true
            };
        }
    }
}