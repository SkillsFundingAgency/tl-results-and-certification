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

        public async Task<DataExportResponse> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy)
        {
            var overallResults = await _dataExportService.DownloadOverallResultsDataAsync(providerUkprn);
            return await ProcessDataExportResponseAsync(overallResults, providerUkprn, DocumentType.OverallResults, DataExportType.NotSpecified, requestedBy, classMapType: typeof(DownloadOverallResultsExportMap), isEmptyFileAllowed: true);
        }

        private async Task<IList<DataExportResponse>> ProcessRegistrationsRequestAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            var registrations = await _dataExportService.GetDataExportRegistrationsAsync(aoUkprn);
            var exportResponse = await ProcessDataExportResponseAsync(registrations, aoUkprn, DocumentType.DataExports, requestType, requestedBy, classMapType: typeof(RegistrationsExportMap));
            return new List<DataExportResponse>() { exportResponse };
        }

        private async Task<IList<DataExportResponse>> ProcessAssessmentsRequestAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            // Core Assessments
            var coreAssessments = await _dataExportService.GetDataExportCoreAssessmentsAsync(aoUkprn);

            // Specialism Assessments
            var specialismAssessments = await _dataExportService.GetDataExportSpecialismAssessmentsAsync(aoUkprn);

            var response = new List<DataExportResponse>();

            var coreAssessmentsResponse = ProcessDataExportResponseAsync(coreAssessments, aoUkprn, DocumentType.DataExports, requestType, requestedBy, ComponentType.Core);
            var specialismAssessmentsResponse = ProcessDataExportResponseAsync(specialismAssessments, aoUkprn, DocumentType.DataExports, requestType, requestedBy, ComponentType.Specialism);

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

            var coreResultsResponse = ProcessDataExportResponseAsync(coreResults, aoUkprn, DocumentType.DataExports, requestType, requestedBy, ComponentType.Core);
            var specialismResultsResponse = ProcessDataExportResponseAsync(specialismResults, aoUkprn, DocumentType.DataExports, requestType, requestedBy, ComponentType.Specialism);

            await Task.WhenAll(coreResultsResponse, specialismResultsResponse);

            response.Add(coreResultsResponse.Result);
            response.Add(specialismResultsResponse.Result);

            return response;
        }

        private async Task<DataExportResponse> ProcessDataExportResponseAsync<T>(IList<T> data, long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified, Type classMapType = null, bool isEmptyFileAllowed = false)
        {
            if (!isEmptyFileAllowed && (data == null || !data.Any()))
                return new DataExportResponse { ComponentType = componentType, IsDataFound = false };

            var byteData = await CsvExtensions.WriteFileAsync(data, classMapType: classMapType);
            var response = await WriteCsvToBlobAsync(ukprn, documentType, requestType, requestedBy, byteData, componentType);
            return response;
        }

        private async Task<DataExportResponse> WriteCsvToBlobAsync(long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, byte[] byteData, ComponentType componentType = ComponentType.NotSpecified)
        {
            // 3. Save to Blob
            var sourceFilePath = documentType == DocumentType.OverallResults ? $"{ukprn}" :
                componentType == ComponentType.NotSpecified ? $"{ukprn}/{requestType}" : $"{ukprn}/{requestType}/{componentType}";

            var blobUniqueReference = Guid.NewGuid();
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = documentType.ToString(),
                SourceFilePath = sourceFilePath,
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