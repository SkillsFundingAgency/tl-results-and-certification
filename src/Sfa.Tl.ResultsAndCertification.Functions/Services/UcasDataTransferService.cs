using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
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
        private readonly ILogger _logger;

        public UcasDataTransferService(IUcasDataService ucasDataService, ILogger<IUcasDataTransferService> logger)
        {
            _ucasDataService = ucasDataService;
            _logger = logger;
        }

        public async Task<UcasDataTransferResponse> ProcessDataTransferAsync()
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
            var list = new List<dynamic> { ucasData.Header };
            ucasData.UcasDataRecords.ToList().ForEach(x => list.Add(x));
            list.Add(ucasData.Trailer);
            var byteData = await CsvExtensions.WriteFileAsync(list);

            // 3. Send data to Ucas using ApiClient
            var filename = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}";
            File.WriteAllBytes(@"c:\temp\" + filename, byteData);

            // 4. Update response
            var response = new UcasDataTransferResponse { IsSuccess = true };
            return response;
        }
    }
}