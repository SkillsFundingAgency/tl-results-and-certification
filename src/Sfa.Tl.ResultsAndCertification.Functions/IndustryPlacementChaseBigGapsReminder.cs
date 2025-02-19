using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class IndustryPlacementChaseBigGapsReminder
    {
        private readonly IIndustryPlacementNotificationService _industryPlacementNotificationService;
        private readonly IPChaseBigGapsReminderExtractSettings _configuration;

        private readonly ICommonService _commonService;

        public IndustryPlacementChaseBigGapsReminder(IIndustryPlacementNotificationService industryPlacementNotificationService, ICommonService commonService, ResultsAndCertificationConfiguration configuration)
        {
            _industryPlacementNotificationService = industryPlacementNotificationService;
            _commonService = commonService;
            _configuration = configuration.IPChaseBigGapsReminderExtractSettings;
        }

        [FunctionName(Constants.IndustryPlacementChaseBigGapsReminder)]
        public async Task IndustryPlacementChaseBigGapsReminderAsync([TimerTrigger("%IndustryPlacementChaseBigGapsReminderTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var today = DateTime.UtcNow.Date;
            bool shouldFunctionRunToday = _configuration.IndustryPlacementChaseBigGapsReminderDateRanges.Any(r => r.Contains(today));

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.IndustryPlacementChaseBigGapsReminder);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _industryPlacementNotificationService.ProcessIndustryPlacementChaseBigGapsReminderAsync();

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                    $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}.\n" +
                                    $"\tTotal users count: {response.UsersCount}\n" +
                                    $"\tNumber of emails sent {response.EmailSentCount}";

                var status = response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed;
                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, status, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                if (status == FunctionStatus.Failed)
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
}