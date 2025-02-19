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
    public class IndustryPlacementMissedDeadlineReminder
    {
        private readonly IIndustryPlacementNotificationService _industryPlacementNotificationService;
        private readonly IPMissedDeadlineReminderSettings _configuration;

        private readonly ICommonService _commonService;

        public IndustryPlacementMissedDeadlineReminder(IIndustryPlacementNotificationService industryPlacementNotificationService, ICommonService commonService, ResultsAndCertificationConfiguration configuration)
        {
            _industryPlacementNotificationService = industryPlacementNotificationService;
            _commonService = commonService;
            _configuration = configuration.IPMissedDeadlineReminderSettings;
        }

        [FunctionName(Constants.IndustryPlacementMissedDeadlineReminder)]
        public async Task IndustryPlacementMissedDeadlineReminderAsync([TimerTrigger("%IndustryPlacementMissedDeadlineReminderTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var today = DateTime.UtcNow.Date;
            bool shouldFunctionRunToday = _configuration.IndustryPlacementMissedDeadlineReminderDateRanges.Any(r => r.Contains(today));

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.IndustryPlacementMissedDeadlineReminder);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _industryPlacementNotificationService.ProcessIndustryPlacementMissedDeadlineReminderAsync();

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