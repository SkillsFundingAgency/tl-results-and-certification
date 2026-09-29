using Microsoft.Azure.Functions.Worker;
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
        private readonly ILogger<OverallResultCalculation> _logger;

        public OverallResultCalculation(ResultsAndCertificationConfiguration configuration, IOverallResultCalculationFunctionService overallResultCalculationService, ICommonService commonService, ILogger<OverallResultCalculation> logger)
        {
            _configuration = configuration;
            _overallResultCalculationService = overallResultCalculationService;
            _commonService = commonService;
            _logger = logger;
        }

        [Function(Constants.OverallResultCalculation)]
        public async Task OverallResultCalculationAsync([TimerTrigger("%OverallResultCalculationTrigger%")] TimerInfo timer, FunctionContext context)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            if (DateTime.UtcNow >= _configuration.OverallResultsCalculationDate)
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionDefinition.Name, FunctionType.OverallResultCalculation);

                try
                {
                    _logger.LogInformation($"Function {context.FunctionDefinition.Name} started");

                    var stopwatch = Stopwatch.StartNew();
                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var responses = await _overallResultCalculationService.CalculateOverallResultsAsync();

                    var message = new StringBuilder($"Function {context.FunctionDefinition.Name} completed processing.").AppendLine();

                    foreach (var (response, index) in responses.Select((value, i) => (value, i)))
                    {
                        message.Append($"Batch {index + 1}: {JsonConvert.SerializeObject(response)}").AppendLine();
                    }

                    var status = responses.All(r => r.IsSuccess) ? FunctionStatus.Processed : responses.All(r => !r.IsSuccess) ? FunctionStatus.Failed : FunctionStatus.PartiallyProcessed;

                    CommonHelper.UpdateFunctionLogRequest(functionLogDetails, status, message.ToString());

                    await _commonService.UpdateFunctionLog(functionLogDetails);

                    // Send Email notification if status is Failed or PartiallyProcessed
                    if (status == FunctionStatus.Failed || status == FunctionStatus.PartiallyProcessed)
                        await _commonService.SendFunctionJobFailedNotification(context.FunctionDefinition.Name, $"Function Status: {status}, Message: {message}");

                    stopwatch.Stop();

                    _logger.LogInformation($"Function {context.FunctionDefinition.Name} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Function {context.FunctionDefinition.Name} failed to process with the following exception = {ex}";
                    _logger.LogError(errorMessage);

                    CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                    _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                    await _commonService.SendFunctionJobFailedNotification(context.FunctionDefinition.Name, errorMessage);
                }
            }
        }
    }
}