using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class CertificatePrinting
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ICertificatePrintingService _certificatePrintingService;
        private readonly ICommonService _commonService;

        public CertificatePrinting(ResultsAndCertificationConfiguration configuration, ICommonService commonService, ICertificatePrintingService certificatePrintingService)
        {
            _configuration = configuration;
            _commonService = commonService;
            _certificatePrintingService = certificatePrintingService;
        }
        
        [FunctionName(Constants.GenerateCertificatePrintingBatches)]
        public async Task GenerateCertificatePrintingBatchesAsync([TimerTrigger("%CertificatePrintingBatchesCreateTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            if (DateTime.UtcNow >= _configuration.CertificatePrintingBatchesCreateStartDate)
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.CertificatePrintingBatchesCreate);

                try
                {
                    logger.LogInformation($"Function {context.FunctionName} started");

                    var stopwatch = Stopwatch.StartNew();
                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var responses = await _certificatePrintingService.ProcessCertificatesForPrintingAsync();

                    var message = new StringBuilder($"Function {context.FunctionName} completed processing.").AppendLine();
                    foreach (var (response, index) in responses.Select((value, i) => (value, i)))
                    {
                        message.Append($"Batch {index + 1}: {JsonConvert.SerializeObject(response)}").AppendLine();
                    }

                    var status = responses.All(r => r.IsSuccess) ? FunctionStatus.Processed : responses.All(r => !r.IsSuccess) ? FunctionStatus.Failed : FunctionStatus.PartiallyProcessed;

                    CommonHelper.UpdateFunctionLogRequest(functionLogDetails, status, message.ToString());

                    await _commonService.UpdateFunctionLog(functionLogDetails);

                    // Send Email notification if status is Failed or PartiallyProcessed
                    if (status == FunctionStatus.Failed || status == FunctionStatus.PartiallyProcessed)
                        await _commonService.SendFunctionJobFailedNotification(context.FunctionName, $"Function Status: {status}, Message: {message}");

                    stopwatch.Stop();

                    logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                    logger.LogError(errorMessage);

                    CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                    _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                    await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
                }
            }
        }
        
        [FunctionName(Constants.FetchCertificatePrintingBatchSummary)]
        public async Task FetchCertificatePrintingBatchSummaryAsync([TimerTrigger("%CertificatePrintingBatchSummaryTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.CertificatePrintingBatchSummary);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _certificatePrintingService.ProcessBatchSummaryAsync();

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                      $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}\n" +
                                      $"\tTotal batches to process: {response.TotalCount}\n" +
                                      $"\tProcessed printing requests: {response.PrintingProcessedCount}\n" +
                                      $"\tModified batches to process: {response.ModifiedCount}\n" +
                                      $"\tRows saved: {response.SavedCount}\n" +
                                      $"\tAdditional message: {response.Message}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = (functionLogDetails.Id > 0) ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
            }
        }
                
        [FunctionName(Constants.SubmitCertificatePrintingRequest)]
        public async Task SubmitCertificatePrintingRequestAsync([TimerTrigger("%CertificatePrintingRequestTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.CertificatePrintingRequest);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _certificatePrintingService.ProcessPrintingRequestAsync();

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                      $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}\n" +
                                      $"\tTotal batches to process: {response.TotalCount}\n" +
                                      $"\tProcessed printing requests: {response.PrintingProcessedCount}\n" +
                                      $"\tModified batches to process: {response.ModifiedCount}\n" +
                                      $"\tRows saved: {response.SavedCount}\n" +
                                      $"\tAdditional message: {response.Message}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = (functionLogDetails.Id > 0) ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
            }
        }
        
        [FunctionName(Constants.FetchCertificatePrintingTrackBatch)]
        public async Task FetchCertificatePrintingTrackBatchAsync([TimerTrigger("%CertificatePrintingTrackBatchTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.CertificatePrintingTrackBatch);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _certificatePrintingService.ProcessTrackBatchAsync();

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                      $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}\n" +
                                      $"\tTotal batches to process: {response.TotalCount}\n" +
                                      $"\tProcessed printing requests: {response.PrintingProcessedCount}\n" +
                                      $"\tModified batch items to process: {response.ModifiedCount}\n" +
                                      $"\tRows saved: {response.SavedCount}\n" +
                                      $"\tAdditional message: {response.Message}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = (functionLogDetails.Id > 0) ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
            }
        }
    }
}
