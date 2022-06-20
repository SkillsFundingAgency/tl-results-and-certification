using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class UcasDataTransfer
    {
        private readonly IUcasDataTransferService _ucasDataTransferService;
        private readonly ICommonService _commonService;

        public UcasDataTransfer(IUcasDataTransferService ucasDataTransferService, ICommonService commonService)
        {
            _ucasDataTransferService = ucasDataTransferService;
            _commonService = commonService;
        }

        [FunctionName(Constants.UcasTransferEntries)]
        public async Task UcasTransferEntriesAsync([TimerTrigger("%UcasTransferEntriesTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            // Check if it is the last Wednesday in June and run the function if it is true
            //if (DateTime.UtcNow.IsLastWeekdayOfMonth(DayOfWeek.Wednesday))
            //{
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName);

                try
                {
                    logger.LogInformation($"Function {context.FunctionName} started");

                    var stopwatch = Stopwatch.StartNew();
                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var response = await _ucasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Entries);

                    var message = $"Function {context.FunctionName} completed processing.\n" +
                                          $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}";

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

                    _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                    await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
                }
            //}
        }

        [Disable]
        [FunctionName(Constants.UcasTransferResultEntries)]
        public async Task UcasTransferResultsAsync([TimerTrigger("%UcasTransferResultsTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();
                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _ucasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Results);

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                      $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}";

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

                _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
            }
        }
    }
}
