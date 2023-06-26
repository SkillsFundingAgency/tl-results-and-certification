using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

        [Disable]

        [FunctionName(Constants.UcasTransferEntries)]
        public async Task UcasTransferEntriesAsync([TimerTrigger("%UcasTransferEntriesTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            // Check if it is the Ucas transfer entries trigger date valid
            if (_commonService.IsUcasTransferEntriesTriggerDateValid())
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.UcasTransferEntries);

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
            }
        }

        [Disable]
        [FunctionName(Constants.UcasTransferResults)]
        public async Task UcasTransferResultsAsync([TimerTrigger("%UcasTransferResultsTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));


            //Check if it is the second Thursday in August and run the function if it is true
            if (_commonService.CurrentDate.IsNthWeekdayOfMonth(DayOfWeek.Thursday, Months.August, 2))
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.UcasTransferResults);

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

        [Disable]
        [FunctionName(Constants.UcasTransferAmendments)]
        public async Task UcasTransferAmendmentsAsync([TimerTrigger("%UcasTransferAmendmentsTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var startDate = _commonService.CurrentDate.GetNthDateOfMonth(DayOfWeek.Friday, Months.August, 3);
            var endDate = _commonService.CurrentDate.GetNthDateOfMonth(DayOfWeek.Friday, Months.October, 1);

            var isValidToRunFunction = _commonService.CurrentDate >= startDate && _commonService.CurrentDate <= endDate;

            if (isValidToRunFunction)
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.UcasTransferAmendments);

                try
                {
                    logger.LogInformation($"Function {context.FunctionName} started");

                    var stopwatch = Stopwatch.StartNew();
                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var response = await _ucasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Amendments);

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
}
