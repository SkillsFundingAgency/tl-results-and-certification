using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class AnalystOverallResultExtraction
    {
        private readonly AnalystOverallResultExtractSettings _configuration;
        private readonly IAnalystOverallResultExtractionService _analystResultExtractionService;
        private readonly ICommonService _commonService;

        public AnalystOverallResultExtraction(
            ResultsAndCertificationConfiguration configuration,
            IAnalystOverallResultExtractionService analystResultExtractionService,
            ICommonService commonService)
        {
            _configuration = configuration.AnalystOverallResultExtractSettings;
            _analystResultExtractionService = analystResultExtractionService;
            _commonService = commonService;
        }

        [Function(Constants.AnalystOverallResultExtract)]
        public async Task AnalystOverallResultExtractAsync([TimerTrigger("%AnalystOverallResultExtractTrigger%")] TimerInfo timer, FunctionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var today = DateTime.UtcNow.Date;
            bool shouldFunctionRunToday = _configuration.ValidDateRanges.Any(r => r.Contains(today));

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionDefinition.Name, FunctionType.AnalystOverallResultExtract);

            try
            {
                logger.LogInformation($"Function {context.FunctionDefinition.Name} started");

                var stopwatch = Stopwatch.StartNew();
                await _commonService.CreateFunctionLog(functionLogDetails);

                FunctionResponse response = await _analystResultExtractionService.ProcessAnalystOverallResultExtractionData();

                FunctionStatus status = response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed;
                var message = $"Function {context.FunctionDefinition.Name} completed processing.{Environment.NewLine}Status: {status}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, status, message);
                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionDefinition.Name} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Function {context.FunctionDefinition.Name} failed to process with the following exception = {ex}";

                logger.LogError(errorMessage);
                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);
                await _commonService.SendFunctionJobFailedNotification(context.FunctionDefinition.Name, errorMessage);
            }
        }
    }
}