using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class SpecialismRommExtract
    {   
        private readonly ISpecialismRommExtractionService _specialismRommExtractionService;
        private readonly ICommonService _commonService;
        private readonly SpecialismRommExtractSettings _configuration;


        public SpecialismRommExtract(ResultsAndCertificationConfiguration configuration, ISpecialismRommExtractionService specialismRommExtractionService, ICommonService commonService)
        {
            _specialismRommExtractionService = specialismRommExtractionService;
            _commonService = commonService;
            _configuration = configuration.SpecialismRommExtractSettings;
        }

        [FunctionName(Constants.SpecialismRommExtract)]
        public async Task SpecialismRommExtractAsync([TimerTrigger("%SpecialismRommExtractTrigger%")] TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var today = DateTime.UtcNow.Date;

            bool shouldFunctionRunToday = _configuration.SpecialismRommValidDateRanges.Any(r => r.Contains(today));

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.SpecialismRommExtract);
            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");
                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _specialismRommExtractionService.ProcessSpecialismRommExtractsAsync(_configuration.SpecialismAssessmentSeriesYearsToProcess);
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
