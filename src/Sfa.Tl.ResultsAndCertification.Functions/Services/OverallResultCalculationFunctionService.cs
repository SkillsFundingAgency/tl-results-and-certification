using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class OverallResultCalculationFunctionService : IOverallResultCalculationFunctionService
    {
        private readonly IOverallResultCalculationService _resultCalculationService;
        private readonly ILogger _logger;

        public OverallResultCalculationFunctionService(
            IOverallResultCalculationService resultCalculationService,
            ILogger<IUcasDataTransferService> logger)
        {
            _resultCalculationService = resultCalculationService;
            _logger = logger;
        }

        //public async Task<FunctionResponse> CalculateOverallGradesAsync()
        //{
        //    // Step 1: Get eligible learners to calculate the results.
        //    // 1.1 GetResultCalculationYear from AssessmentSeries table
        //    var runDate = DateTime.UtcNow;
        //    var resultCalculationYear = _commonService.GetAssessmentSeriesForAsync(runDate).ResultCalculationYear;

        //    for (int year = resultCalculationYear; year > resultCalculationYear - 4; year--) // Each year or all years?
        //    {
        //        // 1.2
        //        var learners = await _resultCalculationService.GetLearnersForOverallGradeCalculation(resultCalculationYear);

        //        // Step 2:
        //        foreach (var learner in learners)
        //        {
        //            // Keep lookupdata ready in beganing of this proc. 
        //        }
        //    }

        //    return new FunctionResponse();
        //}

        public async Task<FunctionResponse> CalculateOverallResultsAsync()
        {
            var rundate = System.DateTime.Now;
            var isSuccess = await _resultCalculationService.CalculateOverallResultsAsync(rundate);
            return new FunctionResponse { IsSuccess = isSuccess };
        }
    }
}