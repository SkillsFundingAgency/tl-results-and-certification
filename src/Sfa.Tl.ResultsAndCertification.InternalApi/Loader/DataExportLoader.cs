using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class DataExportLoader : IDataExportLoader
    {
        private readonly IDataExportRepository _dataExportRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDownloadOverallResultsService _downloadOverallResultsService;
        private readonly IResultSlipsGeneratorService _resultSlipsGeneratorService;

        public DataExportLoader(IDataExportRepository dataExportRepository,
            IBlobStorageService blobStorageService,
            IDownloadOverallResultsService downloadOverallResultsService,
            IResultSlipsGeneratorService resultSlipsGeneratorService)
        {
            _dataExportRepository = dataExportRepository;
            _blobStorageService = blobStorageService;
            _downloadOverallResultsService = downloadOverallResultsService;
            _resultSlipsGeneratorService = resultSlipsGeneratorService;
        }

        public Task<IList<DataExportResponse>> ProcessDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy)
            => requestType switch
            {
                DataExportType.Registrations => ProcessRegistrationsRequestAsync(aoUkprn, requestedBy),
                DataExportType.Assessments => ProcessAssessmentsRequestAsync(aoUkprn, requestedBy),
                DataExportType.Results => ProcessResultsRequestAsync(aoUkprn, requestedBy),
                DataExportType.PendingWithdrawals => ProcessPendingWithdrawalsRequestAsync(aoUkprn, requestedBy),
                _ => null,
            };

        public async Task<DataExportResponse> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy)
        {
            var overallResults = await _downloadOverallResultsService.DownloadOverallResultsDataAsync(providerUkprn, DateTime.UtcNow);
            return await ProcessDataExportResponseAsync(overallResults, providerUkprn, DocumentType.OverallResults, DataExportType.NotSpecified, requestedBy, classMapType: typeof(DownloadOverallResultsExportMap), isEmptyFileAllowed: true);
        }

        public async Task<DataExportResponse> DownloadOverallResultSlipsDataAsync(long providerUkprn, string requestedBy)
        {
            var overallResults = await _downloadOverallResultsService.DownloadOverallResultSlipsDataAsync(providerUkprn, DateTime.UtcNow);
            return await ProcessResultSlipsDataExportResponse(overallResults, providerUkprn, DocumentType.ResultSlips, DataExportType.NotSpecified, requestedBy);
        }

        public async Task<DataExportResponse> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, long profileId, string requestedBy)
        {
            var overallResult = await _downloadOverallResultsService.DownloadLearnerOverallResultSlipsDataAsync(providerUkprn, profileId);
            return await ProcessLeanerResultSlipsDataExportResponse(overallResult, providerUkprn, DocumentType.ResultSlips, DataExportType.NotSpecified, requestedBy);
        }

        private async Task<DataExportResponse> ProcessResultSlipsDataExportResponse<T>(IList<T> data, long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified)
        {
            Byte[] byteData = _resultSlipsGeneratorService.GetByteData(data.Cast<DownloadOverallResultSlipsData>());
            return await WritePdfToBlobAsync(ukprn, documentType, requestType, requestedBy, byteData, componentType);
        }

        private async Task<DataExportResponse> ProcessLeanerResultSlipsDataExportResponse(DownloadOverallResultSlipsData data, long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified)
        {
            List<DownloadOverallResultSlipsData> downloadOverallResultSlipsDatas = new() { data };

            Byte[] byteData = _resultSlipsGeneratorService.GetByteData(downloadOverallResultSlipsDatas);
            return await WritePdfToBlobAsync(ukprn, documentType, requestType, requestedBy, byteData, componentType);
        }

        public async Task<IList<DataExportResponse>> DownloadRommExportAsync(long aoUkprn, string requestedBy)
        {
            var romms = await _dataExportRepository.GetDataExportRommsAsync(aoUkprn);

            var exportResponse = await ProcessRommsExportResponseAsync(romms, aoUkprn, DocumentType.DataExports, DataExportType.Romms, requestedBy, classMapType: typeof(RommsExportMap));
            return new List<DataExportResponse>() { exportResponse };
        }

        private async Task<DataExportResponse> ProcessRommsExportResponseAsync<T>(IList<T> data, long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified, Type classMapType = null, bool isEmptyFileAllowed = false)
        {
            if (!isEmptyFileAllowed && (data == null || !data.Any()))
                return new DataExportResponse { ComponentType = componentType, IsDataFound = false };

            var byteData = await CsvExtensions.WriteFileAsync(data, classMapType: classMapType);
            var response = await WriteCsvToBlobAsync(ukprn, documentType, requestType, requestedBy, byteData, componentType);
            return response;
        }

        private async Task<IList<DataExportResponse>> ProcessRegistrationsRequestAsync(long aoUkprn, string requestedBy)
        {
            var registrations = await _dataExportRepository.GetDataExportRegistrationsAsync(aoUkprn);
            var exportResponse = await ProcessDataExportResponseAsync(registrations, aoUkprn, DocumentType.DataExports, DataExportType.Registrations, requestedBy, classMapType: typeof(RegistrationsExportMap));
            return new List<DataExportResponse>() { exportResponse };
        }

        private async Task<IList<DataExportResponse>> ProcessAssessmentsRequestAsync(long aoUkprn, string requestedBy)
        {
            // Core Assessments
            var coreAssessments = await _dataExportRepository.GetDataExportCoreAssessmentsAsync(aoUkprn);

            // Specialism Assessments
            var specialismAssessments = await _dataExportRepository.GetDataExportSpecialismAssessmentsAsync(aoUkprn);

            var response = new List<DataExportResponse>();

            var coreAssessmentsResponse = ProcessDataExportResponseAsync(coreAssessments, aoUkprn, DocumentType.DataExports, DataExportType.Assessments, requestedBy, ComponentType.Core);
            var specialismAssessmentsResponse = ProcessDataExportResponseAsync(specialismAssessments, aoUkprn, DocumentType.DataExports, DataExportType.Assessments, requestedBy, ComponentType.Specialism);

            await Task.WhenAll(coreAssessmentsResponse, specialismAssessmentsResponse);

            response.Add(coreAssessmentsResponse.Result);
            response.Add(specialismAssessmentsResponse.Result);

            return response;
        }

        private async Task<IList<DataExportResponse>> ProcessResultsRequestAsync(long aoUkprn, string requestedBy)
        {
            // Core Results
            var coreResults = await _dataExportRepository.GetDataExportCoreResultsAsync(aoUkprn);

            // Specialism Results
            var specialismResults = await _dataExportRepository.GetDataExportSpecialismResultsAsync(aoUkprn);

            var response = new List<DataExportResponse>();

            var coreResultsResponse = ProcessDataExportResponseAsync(coreResults, aoUkprn, DocumentType.DataExports, DataExportType.Results, requestedBy, ComponentType.Core);
            var specialismResultsResponse = ProcessDataExportResponseAsync(specialismResults, aoUkprn, DocumentType.DataExports, DataExportType.Results, requestedBy, ComponentType.Specialism);

            await Task.WhenAll(coreResultsResponse, specialismResultsResponse);

            response.Add(coreResultsResponse.Result);
            response.Add(specialismResultsResponse.Result);

            return response;
        }

        private async Task<IList<DataExportResponse>> ProcessPendingWithdrawalsRequestAsync(long aoUkprn, string requestedBy)
        {
            IList<PendingWithdrawalsExport> pendingWithdrawals = await _dataExportRepository.GetDataExportPendingWithdrawalsAsync(aoUkprn);

            DataExportResponse exportResponse = await ProcessDataExportResponseAsync(pendingWithdrawals, aoUkprn, DocumentType.DataExports, DataExportType.PendingWithdrawals, requestedBy, classMapType: typeof(PendingWithdrawalsExportMap));
            return new List<DataExportResponse>() { exportResponse };
        }

        private async Task<DataExportResponse> ProcessDataExportResponseAsync<T>(IList<T> data, long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, ComponentType componentType = ComponentType.NotSpecified, Type classMapType = null, bool isEmptyFileAllowed = false)
        {
            if (!isEmptyFileAllowed && (data == null || !data.Any()))
                return new DataExportResponse { ComponentType = componentType, IsDataFound = false };

            var byteData = await CsvExtensions.WriteFileAsync(data, classMapType: classMapType);
            var response = await WriteCsvToBlobAsync(ukprn, documentType, requestType, requestedBy, byteData, componentType);
            return response;
        }

        private async Task<DataExportResponse> WritePdfToBlobAsync(long ukprn, DocumentType documentType, DataExportType requestType, string requestedBy, byte[] byteData, ComponentType componentType = ComponentType.NotSpecified)
        {
            // 3. Save to Blob
            var sourceFilePath = documentType == DocumentType.ResultSlips ? $"{ukprn}" :
                componentType == ComponentType.NotSpecified ? $"{ukprn}/{requestType}" : $"{ukprn}/{requestType}/{componentType}";

            var blobUniqueReference = Guid.NewGuid();
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = documentType.ToString(),
                SourceFilePath = sourceFilePath,
                BlobFileName = $"{blobUniqueReference}.{FileType.Pdf}",
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