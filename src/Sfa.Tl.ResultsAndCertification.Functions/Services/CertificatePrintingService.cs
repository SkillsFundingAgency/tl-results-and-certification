using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class CertificatePrintingService : ICertificatePrintingService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPrinitingApiClient _printingApiClient;
        private readonly IPrintingService _printingService;

        public CertificatePrintingService(IMapper mapper, ILogger<ICertificatePrintingService> logger, IPrinitingApiClient printingApiClient, IPrintingService printingService)
        {
            _mapper = mapper;
            _logger = logger;
            _printingApiClient = printingApiClient;
            _printingService = printingService;
        }

        public async Task<CertificatePrintingResponse> ProcessPrintRequestAsync()
        {
            // service call to get printing request
            var pendingPrintRequests = await _printingService.GetPendingPrintRequestsAsync();

            if (pendingPrintRequests == null || !pendingPrintRequests.Any())
            {
                var message = $"No pending batches found to process print request. Method: ProcessPrintRequestAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new CertificatePrintingResponse { IsSuccess = true, Message = message };
            }

            var printRequestResponses = new List<PrintRequestResponse>();

            //// post data api and get respone
            //foreach (var pendingPrintRequest in pendingPrintRequests)
            //{
            //    var printRequestResponse = await _printingApiClient.ProcessPrintRequestAsync(pendingPrintRequest);

            //    if (printRequestResponse != null)
            //        printRequestResponses.Add(printRequestResponse);
            //}

            // update batch based on response -- service call to update
            var response = await _printingService.UpdatePrintReqeustResponsesAsync(printRequestResponses);
            response.TotalCount = pendingPrintRequests.Count();
            return response;
        }
    }
}
