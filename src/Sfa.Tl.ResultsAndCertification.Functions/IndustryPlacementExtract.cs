using Microsoft.Azure.Functions.Worker;
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
    public class IndustryPlacementExtract
    {
        private readonly IIndustryPlacementService _industryPlacementService;

        private readonly ICommonService _commonService;

        public IndustryPlacementExtract(IIndustryPlacementService industryPlacementService, ICommonService commonService)
        {
            _industryPlacementService = industryPlacementService;
            _commonService = commonService;
        }

        [Function(Constants.IndustryPlacementExtract)]
        public async Task IndustryPlacementExtractAsync([TimerTrigger("%IndustryPlacementExtractTrigger%")] TimerInfo timer, FunctionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            if (_commonService.IsIndustryPlacementTriggerDateValid())
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionDefinition.Name, FunctionType.IndustryPlacementExtract);

                try
                {
                    logger.LogInformation($"Function {context.FunctionDefinition.Name} started");

                    var stopwatch = Stopwatch.StartNew();

                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var response = await _industryPlacementService.ProcessIndustryPlacementExtractionsAsync();
                    var message = $"Function {context.FunctionDefinition.Name} completed processing.\n" +
                                         $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}";

                    CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

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
}