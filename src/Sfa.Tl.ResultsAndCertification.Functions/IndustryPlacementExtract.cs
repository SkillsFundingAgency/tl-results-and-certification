using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;


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

                var response = await _industryPlacementService.ProcessIndustryPlacementExtractionsAsync();
;
            }


                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
        }
    }
}
