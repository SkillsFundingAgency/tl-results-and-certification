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
            return requestType switch
            {
                DataExportType.Registrations => await ProcessRegistrationsRequestAsync(aoUkprn, requestType, requestedBy),
                DataExportType.Assessments => await ProcessAssessmentsRequestAsync(aoUkprn, requestType, requestedBy),
                DataExportType.Results => await ProcessResultsRequestAsync(aoUkprn, requestType, requestedBy),
                _ => null,
            };
        }

        private async Task<IList<DataExportResponse>> ProcessRegistrationsRequestAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            var registrations = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn);
            var exportResponse = await ProcessDataExportResponseAsync(registrations, aoUkprn, requestType, requestedBy, classMapType: typeof(RegistrationsExportMap));
            return new List<DataExportResponse>() { exportResponse };
        }

        private async Task<IList<DataExportResponse>> ProcessAssessmentsRequestAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            // Core Assessments
            var coreAssessments = await _dataExportService.GetDataExportCoreAssessmentsAsync(aoUkprn);

            // Specialism Assessments
            var specialismAssessments = await _dataExportService.GetDataExportSpecialismAssessmentsAsync(aoUkprn);

            var response = new List<DataExportResponse>();

            var coreAssessmentsResponse = ProcessDataExportResponseAsync(coreAssessments, aoUkprn, requestType, requestedBy, ComponentType.Core);
            var specialismAssessmentsResponse = ProcessDataExportResponseAsync(specialismAssessments, aoUkprn, requestType, requestedBy, ComponentType.Specialism);

            await Task.WhenAll(coreAssessmentsResponse, specialismAssessmentsResponse);

            response.Add(coreAssessmentsResponse.Result);
            response.Add(specialismAssessmentsResponse.Result);

            return response;
        }

        private async Task<IList<DataExportResponse>> ProcessResultsRequestAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            // Core Results
            var coreResults = await _dataExportService.GetDataExportCoreResultsAsync(aoUkprn);

            // Specialism Results
            var specialismResults = await _dataExportService.GetDataExportSpecialismResultsAsync(aoUkprn);

            var response = new List<DataExportResponse>();

            var coreResultsResponse = ProcessDataExportResponseAsync(coreResults, aoUkprn, requestType, requestedBy, ComponentType.Core);
            var specialismResultsResponse = ProcessDataExportResponseAsync(specialismResults, aoUkprn, requestType, requestedBy, ComponentType.Specialism);

            await Task.WhenAll(coreResultsResponse, specialismResultsResponse);

            response.Add(coreResultsResponse.Result);
            response.Add(specialismResultsResponse.Result);

            return response;
        }

        private async Task<DataExportResponse> ProcessDataExportResponseAsync<T>(IList<T> data, long aoUkprn, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified, Type classMapType = null)
        {
            if (data == null || !data.Any())
                return new DataExportResponse { ComponentType = componentType, IsDataFound = false };

            var byteData = await CsvExtensions.WriteFileAsync(data, classMapType: classMapType);
            var response = await WriteCsvToBlobAsync(aoUkprn, requestType, requestedBy, byteData, componentType);
            return response;
        }

        private async Task<DataExportResponse> WriteCsvToBlobAsync(long aoUkprn, DataExportType requestType, string requestedBy, byte[] byteData, ComponentType componentType = ComponentType.NotSpecified)
        {
            // 3. Save to Blob
            var blobUniqueReference = Guid.NewGuid();
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                SourceFilePath = componentType == ComponentType.NotSpecified ? $"{aoUkprn}/{requestType}" : $"{aoUkprn}/{requestType}/{componentType}",
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = byteData,
                UserName = requestedBy
            });

            // 4. Return response
            return new DataExportResponse
            {
                FileSize = Math.Round((byteData.Length / 1024D), 2),
                BlobUniqueReference = blobUniqueReference,
                ComponentType = componentType,
                IsDataFound = true
            };
        }
    }
}