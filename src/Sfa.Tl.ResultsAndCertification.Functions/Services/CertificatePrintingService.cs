using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class CertificatePrintingService : ICertificatePrintingService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPrintingApiClient _printingApiClient;
        private readonly IPrintingService _printingService;
        private readonly ICertificateService _certificateService;

        public CertificatePrintingService(IMapper mapper,
            ILogger<ICertificatePrintingService> logger,
            IPrintingApiClient printingApiClient,
            IPrintingService printingService,
            ICertificateService certificateService)
        {
            _mapper = mapper;
            _logger = logger;
            _printingApiClient = printingApiClient;
            _printingService = printingService;
            _certificateService = certificateService;
        }

        public async Task<CertificatePrintingResponse> ProcessPrintingRequestAsync()
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

            // post data api and get respone
            foreach (var pendingPrintRequest in pendingPrintRequests)
            {
                if (pendingPrintRequest.Batch.TotalCertificateCount == 0) continue; // If the certificate count is 0 dont call the xerox API
                var printResponse = await _printingApiClient.ProcessPrintRequestAsync(pendingPrintRequest);

                if (printResponse != null)
                {
                    printResponse.PrintRequestResponse.BatchId = pendingPrintRequest.Batch.BatchNumber;
                    printRequestResponses.Add(printResponse.PrintRequestResponse);
                }
            }

            // update batch based on response -- service call to update
            var response = await _printingService.UpdatePrintRequestResponsesAsync(printRequestResponses);
            response.TotalCount = pendingPrintRequests.Count();
            return response;
        }

        public async Task<CertificatePrintingResponse> ProcessBatchSummaryAsync()
        {
            // service call to get printing request
            var batchIds = await _printingService.GetPendingPrintBatchesForBatchSummaryAsync();

            if (batchIds == null || !batchIds.Any())
            {
                var message = $"No pending print batches found to process batch summary. Method: ProcessBatchSummaryAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new CertificatePrintingResponse { IsSuccess = true, Message = message };
            }

            var printBatchSummaryResponses = new List<BatchSummaryResponse>();

            // post data api and get respone
            foreach (var batchId in batchIds)
            {
                var batchSummaryResponse = await _printingApiClient.GetBatchSummaryInfoAsync(batchId);

                if (batchSummaryResponse != null)
                    printBatchSummaryResponses.Add(batchSummaryResponse);
            }

            // update print batch based on response -- service call to update
            var response = await _printingService.UpdateBatchSummaryResponsesAsync(printBatchSummaryResponses);
            response.TotalCount = batchIds.Count();
            return response;
        }

        public async Task<CertificatePrintingResponse> ProcessTrackBatchAsync()
        {
            // service call to get printing request
            var batchIds = await _printingService.GetPendingItemsForTrackBatchAsync();

            if (batchIds == null || !batchIds.Any())
            {
                var message = $"No pending items found to process track batch. Method: ProcessTrackBatchAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new CertificatePrintingResponse { IsSuccess = true, Message = message };
            }

            var trackBatchResponses = new List<TrackBatchResponse>();

            // post data api and get respone
            foreach (var batchId in batchIds)
            {
                var trackBatchResponse = await _printingApiClient.GetTrackBatchInfoAsync(batchId);

                if (trackBatchResponse != null)
                    trackBatchResponses.Add(trackBatchResponse);
            }

            // update print batch items based on response -- service call to update
            var response = await _printingService.UpdateTrackBatchResponsesAsync(trackBatchResponses);
            response.TotalCount = batchIds.Count();
            return response;
        }

        public async Task<List<CertificateResponse>> ProcessCertificatesForPrintingAsync()
        {
            var response = await _certificateService.ProcessCertificatesForPrintingAsync();

            if (response == null || !response.Any())
            {
                var message = "No learners data retrieved to process certificates for printing. Method: ProcessCertificatesForPrintingAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new List<CertificateResponse> { new CertificateResponse { IsSuccess = true, Message = message } };
            }
            return response;
        }
    }
}
