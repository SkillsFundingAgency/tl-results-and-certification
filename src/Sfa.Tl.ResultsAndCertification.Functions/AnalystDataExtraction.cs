using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class AnalystDataExtraction
    {
        private readonly IAnalystResultExtractionService _analystResultExtractionService;
        private readonly ICommonService _commonService;

        public AnalystDataExtraction(
            IAnalystResultExtractionService analystResultExtractionService,
            ICommonService commonService)
        {
            _analystResultExtractionService = analystResultExtractionService;
            _commonService = commonService;
        }

        [FunctionName(Constants.AnalystOverallResultExtract)]
        public async Task AnalystOverallResultExtractAsync([TimerTrigger("%AnalystOverallResultExtractTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.AnalystOverallResultExtract);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();
                await _commonService.CreateFunctionLog(functionLogDetails);

                await _analystResultExtractionService.ProcessAnalystOverallResultExtractionData(new int[] { 2020 });

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Processed, $"Function {context.FunctionName} completed processing.");
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