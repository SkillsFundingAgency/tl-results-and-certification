using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class SpecialismRommExtract
    {
        private readonly ISpecialismRommExtractionService _specialismRommExtractionService;
        private readonly ICommonService _commonService;
        private readonly SpecialismRommExtractSettings _configuration;
        private readonly ILogger _logger;

        public SpecialismRommExtract(ResultsAndCertificationConfiguration configuration, ISpecialismRommExtractionService specialismRommExtractionService, ICommonService commonService, ILogger logger)
        {
            _specialismRommExtractionService = specialismRommExtractionService;
            _commonService = commonService;
            _configuration = configuration.SpecialismRommExtractSettings;
            _logger = logger;
        }

        [Function(Constants.SpecialismRommExtract)]
        public async Task SpecialismRommExtractAsync([TimerTrigger("%SpecialismRommExtractTrigger%")] TimerInfo timer, FunctionContext context)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var today = DateTime.UtcNow.Date;

            bool shouldFunctionRunToday = _configuration.SpecialismRommValidDateRanges.Any(r => r.Contains(today));

            if (!shouldFunctionRunToday)
            {
                await Task.CompletedTask;
                return;
            }

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionDefinition.Name, FunctionType.SpecialismRommExtract);
            try
            {
                _logger.LogInformation($"Function {context.FunctionDefinition.Name} started");
                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _specialismRommExtractionService.ProcessSpecialismRommExtractsAsync(_configuration.SpecialismAssessmentSeriesYearsToProcess);
                var message = $"Function {context.FunctionDefinition.Name} completed processing.\n" +
                                     $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

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