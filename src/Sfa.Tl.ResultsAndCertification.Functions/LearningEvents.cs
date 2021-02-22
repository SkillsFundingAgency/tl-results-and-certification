using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class LearningEvents
    {
        private readonly IPersonalLearningRecordService _personalLearningRecordService;
        private readonly ICommonService _commonService;

        public LearningEvents(ICommonService commonService, IPersonalLearningRecordService personalLearningRecordService)
        {
            _commonService = commonService;
            _personalLearningRecordService = personalLearningRecordService;
        }

        [FunctionName("GetLearningEvents")]
        public async Task GetLearningEventsAsync([TimerTrigger("%LearningEventsTrigger%")]TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                await _personalLearningRecordService.ProcessLearnerVerificationAndLearningEvents();

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Processed);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch(Exception ex)
            {
                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = (functionLogDetails.Id > 0) ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);
                
                throw;
            }
        }
    }
}
