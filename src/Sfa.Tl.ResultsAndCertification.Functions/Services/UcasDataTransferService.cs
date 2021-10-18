using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class UcasDataTransferService : IUcasDataTransferService
    {
        private readonly IUcasDataService _ucasDataService;
        private readonly IUcasApiClient _ucasApiClient;
        private readonly ILogger _logger;

        public UcasDataTransferService(IUcasDataService ucasDataService, IUcasApiClient ucasApiClient, ILogger<IUcasDataTransferService> logger)
        {
            _ucasDataService = ucasDataService;
            _ucasApiClient = ucasApiClient;
            _logger = logger;
        }

        public async Task<UcasDataTransferResponse> ProcessUcasEntriesAsync()
        {
            // 1. Get Entries data
            var ucasData = await _ucasDataService.GetUcasEntriesAsync();
            if (ucasData == null || !ucasData.UcasDataRecords.Any())
            {
                var message = $"No entries are found. Method: GetUcasEntriesAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new UcasDataTransferResponse { IsSuccess = true, Message = message };
            }

            // 2. Write to the file (in byte format)
            var ucasDataRecords = new List<dynamic> { ucasData.Header };
            ucasDataRecords.AddRange(ucasData.UcasDataRecords);
            ucasDataRecords.Add(ucasData.Trailer);

            var byteData = await CsvExtensions.WriteFileAsync(ucasDataRecords, typeof(CsvMapper));

            // 3. Send data to Ucas using ApiClient
            var filename = $"{Guid.NewGuid()}.{Constants.FileExtensionTxt}";
            var fileHash = CommonHelper.ComputeSha256Hash(byteData);

            // 4. Call Ucas Api client
            var ucasFileId = await _ucasApiClient.SendDataAsync(new UcasDataRequest { FileName = filename, FileData = byteData, FileHash = fileHash });

            File.WriteAllBytes(@"c:\temp\" + filename, byteData); // Need to remove after implementing Blob storage

            // Need to write the file to Blob
            var blobFileName = $"{ucasFileId}_{filename}";

            // 4. Update response
            return new UcasDataTransferResponse { IsSuccess = true };
        }
    }
}