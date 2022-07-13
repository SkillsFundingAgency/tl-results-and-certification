using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class OverallResultCalculationFunctionService : IOverallResultCalculationFunctionService
    {
        private readonly IOverallResultCalculationService _resultCalculationService;
        private readonly ILogger _logger;

        public OverallResultCalculationFunctionService(
            IOverallResultCalculationService resultCalculationService,
            ILogger<IOverallResultCalculationFunctionService> logger)
        {
            _resultCalculationService = resultCalculationService;
            _logger = logger;
        }

        public async Task<IList<OverallResultResponse>> CalculateOverallResultsAsync()
        {
            var runDate = DateTime.UtcNow;
            var response = await _resultCalculationService.CalculateOverallResultsAsync(runDate);

            if (response == null || !response.Any())
            {
                var message = $"No learners data retrieved to process overall result calculation. Method: CalculateOverallResultsAsync({runDate})";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new List<OverallResultResponse> { new OverallResultResponse { IsSuccess = true, Message = message } };
            }
            return response;
        }
    }
}