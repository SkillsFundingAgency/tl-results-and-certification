﻿using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class DownloadOverallResultsLoader : IDownloadOverallResultsLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DownloadOverallResultsLoader> _logger;


        public DownloadOverallResultsLoader(IResultsAndCertificationInternalApiClient internalApiClient,
            IBlobStorageService blobStorageService,
            ILogger<DownloadOverallResultsLoader> logger)
        {
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<Stream> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy)
        {
            var apiResponse = await _internalApiClient.DownloadOverallResultsDataAsync(providerUkprn, requestedBy);
            if (apiResponse == null || apiResponse.BlobUniqueReference == Guid.Empty)
                return null;

            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.OverallResults.ToString(),
                SourceFilePath = $"{providerUkprn}",
                BlobFileName = $"{apiResponse.BlobUniqueReference}.{FileType.Csv}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download overallresults data. Method: DownloadOverallResultsDataAsync(ContainerName: {DocumentType.OverallResults}, BlobFileName = {apiResponse.BlobUniqueReference}, SourceFilePath = {providerUkprn})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }

        public async Task<Stream> DownloadOverallResultSlipsAsync(long providerUkprn)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.ResultSlips.ToString(),
                SourceFilePath = $"{providerUkprn}",
                BlobFileName = $"{providerUkprn}.{FileType.Pdf}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download overall result slips. Method: DownloadOverallResultSlipsAsync(ContainerName: {DocumentType.ResultSlips}, BlobFileName = {providerUkprn}.{FileType.Pdf}, SourceFilePath = {providerUkprn})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }
    }
}
