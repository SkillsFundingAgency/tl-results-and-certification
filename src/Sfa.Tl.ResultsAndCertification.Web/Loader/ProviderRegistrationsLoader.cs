using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Web.Content.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ProviderRegistrationsLoader : IProviderRegistrationsLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderRegistrationsLoader> _logger;

        public ProviderRegistrationsLoader(
            IResultsAndCertificationInternalApiClient internalApiClient,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            ILogger<ProviderRegistrationsLoader> logger)
        {
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<AvailableStartYearViewModel>> GetAvailableStartYearsAsync()
        {
            IList<int> availableYears = await _internalApiClient.GetProviderRegistrationsAvailableStartYearsAsync();
            return _mapper.Map<IList<AvailableStartYearViewModel>>(availableYears);
        }

        public Task<DataExportResponse> GetRegistrationsDataExportAsync(long providerUkprn, int startYear, string requestedBy)
        {
            var request = new GetProviderRegistrationsRequest
            {
                ProviderUkprn = providerUkprn,
                StartYear = startYear,
                RequestedBy = requestedBy
            };

            return _internalApiClient.GetProviderRegistrationsAsync(request);
        }

        public DownloadRegistrationsDataForViewModel GetDownloadRegistrationsDataForViewModel(DataExportResponse dataExportResponse, int startYear)
            => _mapper.Map<DownloadRegistrationsDataForViewModel>(dataExportResponse, opt => opt.Items["start-year"] = startYear);

        public async Task<FileStreamResult> GetRegistrationsFileAsync(long providerUkprn, Guid blobUniqueReference)
        {
            string sourceFilePath = $"{providerUkprn}/{DataExportType.ProviderRegistrations}";

            Stream fileStream = await DownloadFileAsync(blobUniqueReference, sourceFilePath);

            if (fileStream == null)
            {
                string blobReadError = $"No FileStream found to download provider registrations data. Method: {nameof(GetRegistrationsFileAsync)} (ContainerName: {DocumentType.DataExports}, BlobFileName = {blobUniqueReference}, SourceFilePath = {sourceFilePath})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/csv")
            {
                FileDownloadName = DownloadRegistrationsDataFor.Registrations_Data_Report_File_Name_Text
            };
        }

        private Task<Stream> DownloadFileAsync(Guid blobUniqueReference, string sourceFilePath)
        {
            return _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                SourceFilePath = sourceFilePath
            });
        }
    }
}