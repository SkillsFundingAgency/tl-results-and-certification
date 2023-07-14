using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;


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

        [Disable]
        [FunctionName(Constants.IndustryPlacementExtract)]
        public async Task IndustryPlacementExtractAsync([TimerTrigger("%IndustryPlacementExtractTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            if (_commonService.IsIndustryPlacementTriggerDateValid())
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.IndustryPlacementExtract);


                try
                {

                    logger.LogInformation($"Function {context.FunctionName} started");

                    var stopwatch = Stopwatch.StartNew();

                    await _commonService.CreateFunctionLog(functionLogDetails);

                    var response = await _industryPlacementService.ProcessIndustryPlacementExtractionsAsync();
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