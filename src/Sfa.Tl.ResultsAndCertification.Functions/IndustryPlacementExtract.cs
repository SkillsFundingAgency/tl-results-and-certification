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
        private readonly ICommonService _commonService;

        public IndustryPlacementExtract(ICommonService commonService)
        {
            _commonService = commonService;
        }


        [FunctionName(Constants.IndustryPlacementExtract)]
        public async Task IndustryPlacementExtractAsync([TimerTrigger("%IndustryPlacementExtractTrigger%")]TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));            

            if (_commonService.IsUcasTransferEntriesTriggerDateValid())
            {
                var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.UcasTransferEntries);

                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);
            }


                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
        }
    }
}
