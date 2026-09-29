using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class CertificateTrackingExtraction
    {
        private readonly ICertificateTrackingExtractionService _service;
        private readonly ICommonService _commonService;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public CertificateTrackingExtraction(
            ICertificateTrackingExtractionService service,
            ICommonService commonService,
            ResultsAndCertificationConfiguration configuration)
        {
            _service = service;
            _commonService = commonService;
            _configuration = configuration;
        }

        [Function(Constants.CertificateTrackingExtract)]
        public async Task CertificateTrackingExtractAsync([TimerTrigger("%CertificateTrackingExtractTrigger%")] TimerInfo timer, FunctionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionDefinition.Name, FunctionType.CertificateTrackingExtract);

            try
            {
                logger.LogInformation($"Function {context.FunctionDefinition.Name} started");

                var stopwatch = Stopwatch.StartNew();
                await _commonService.CreateFunctionLog(functionLogDetails);

                int numberOfDaysToProcess = _configuration.CertificateTrackingExtractSettings.NumberOfDays;
                DateTime fromDay = DateTime.Today.SubtractDays(numberOfDaysToProcess);

                FunctionResponse response = await _service.ProcessCertificateTrackingExtractAsync(() => fromDay, () => $"{Guid.NewGuid()}.{FileType.Csv}");

                FunctionStatus status = response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed;
                var message = $"Function {context.FunctionDefinition.Name} completed processing.{Environment.NewLine}Status: {status}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, status, message);
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