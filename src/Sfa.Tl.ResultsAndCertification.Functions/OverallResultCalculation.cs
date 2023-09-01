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
    public class OverallResultCalculation
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IOverallResultCalculationFunctionService _overallResultCalculationService;
        private readonly ICommonService _commonService;

        public OverallResultCalculation(ResultsAndCertificationConfiguration configuration, IOverallResultCalculationFunctionService overallResultCalculationService, ICommonService commonService)
        {
            _configuration = configuration;
            _overallResultCalculationService = overallResultCalculationService;
            _commonService = commonService;
        }
        
        [FunctionName(Constants.OverallResultCalculation)]
        public async Task OverallResultCalculationAsync([TimerTrigger("%OverallResultCalculationTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            if (DateTime.UtcNow >= _configuration.OverallResultsCalculationDate)
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.OverallResultCalculation);

                try
                {
                    logger.LogInformation($"Function {context.FunctionName} started");

                    var stopwatch = Stopwatch.StartNew();
                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var responses = await _overallResultCalculationService.CalculateOverallResultsAsync();

                    var message = new StringBuilder($"Function {context.FunctionName} completed processing.").AppendLine();

                    foreach (var (response, index) in responses.Select((value, i) => (value, i)))
                    {
                        message.Append($"Batch {index + 1}: { JsonConvert.SerializeObject(response)}").AppendLine();
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
    }
}
