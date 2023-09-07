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

            logger.LogInformation($"Function SpecialismRommExtractTrigger {context.FunctionName} started");

            logger.LogInformation($"_configuration started");

            logger.LogInformation($"_configuration.SpecialismRommValidDateRanges {_configuration}");


            logger.LogInformation($"_configuration.SpecialismRommValidDateRanges {_configuration.SpecialismRommValidDateRanges} SpecialismRommValidDateRanges Object");

            logger.LogInformation($"_configuration.SpecialismRommValidDateRanges {_configuration.SpecialismRommValidDateRanges?.Count()} count");


            logger.LogInformation($"_configuration.SpecialismRommValidDateRanges {_configuration.SpecialismRommValidDateRanges?.FirstOrDefault().From.ToString()} From Date");

            logger.LogInformation($"_configuration.SpecialismRommValidDateRanges {_configuration.SpecialismRommValidDateRanges?.FirstOrDefault().To.ToString()} To Date");


            bool shouldFunctionRunToday = _configuration.SpecialismRommValidDateRanges.Any(r => r.Contains(today));

            logger.LogInformation($"Function SpecialismRommExtractTrigger shouldFunctionRunTodaystarted {shouldFunctionRunToday}");

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            logger.LogInformation($"Function ran configs succesfully");

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName, FunctionType.SpecialismRommExtract);
            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");
                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                logger.LogInformation($"_commonService  created");

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
                logger.LogInformation($"Exception {ex.ToString}");

                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = functionLogDetails.Id > 0 ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);

                await _commonService.SendFunctionJobFailedNotification(context.FunctionName, errorMessage);
            }

        }
    }
}
