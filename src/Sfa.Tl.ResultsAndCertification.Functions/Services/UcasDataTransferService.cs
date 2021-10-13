using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class UcasDataTransferService : IUcasDataTransferService
    {
        private readonly IUcasDataService _ucasDataService;

        public UcasDataTransferService(IUcasDataService ucasDataService)
        {
            _ucasDataService = ucasDataService;
        }

        public async Task<UcasDataTransferResponse> ProcessDataTransferAsync()
        {
            var ucasData = await _ucasDataService.GetUcasEntriesAsync();

            var list = new List<dynamic> { ucasData.Header };
            ucasData.UcasDataRecords.ToList().ForEach(x => list.Add(x));
            list.Add(ucasData.Trailer);

            var byteData = await CsvExtensions.WriteFileAsync(list);
            File.WriteAllBytes(@"c:\temp\test.csv", byteData);

            var response = new UcasDataTransferResponse { IsSuccess = true };
            return response;
        }
    }
}