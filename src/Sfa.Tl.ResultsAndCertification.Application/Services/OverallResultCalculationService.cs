using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class OverallResultCalculationService : IOverallResultCalculationService
    {
        private readonly IOverallResultCalculationRepository _overallGradeCalculationRepository;
        private readonly IAssessmentService _assessmentService;

        public OverallResultCalculationService(
            IOverallResultCalculationRepository overallGradeCalculationRepository, 
            IAssessmentService assessmentService)
        {
            _overallGradeCalculationRepository = overallGradeCalculationRepository;
            _assessmentService = assessmentService;
        }

        public async Task<int> GetResultCalculationYearOfAsync(DateTime runDate)
        {
            var assessmentSeries = await _assessmentService.GetAssessmentSeriesAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => runDate >= a.StartDate && runDate <= a.EndDate);

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment.ResultCalculationYear;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            var resultCalculationYear = await GetResultCalculationYearOfAsync(runDate);
            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYear);
        }

        public async Task<bool> CalculateOverallResultsAsync(DateTime runDate)
        {
            var learners = await GetLearnersForOverallGradeCalculationAsync(runDate);

            foreach (var learner in learners)
            {
                var coreGrade = await GetHightestCoreGradeAsync(learner);
                var specialismGrade = await GetHightestSpecialismGradeAsync(learner);
                var ipStatus = learner.IndustryPlacements.Any() ? learner.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;

                var overallGrade = await GetOverAllGradeAsync(coreGrade, specialismGrade, ipStatus);

                // Save.
            }

            return true;
        }

        
        private async Task<int> GetHightestCoreGradeAsync(TqRegistrationPathway learner)
        {
            await Task.CompletedTask;
            return 1;
        }
        
        private async Task<int> GetHightestSpecialismGradeAsync(TqRegistrationPathway learner)
        {
            await Task.CompletedTask;
            return 1;
        }

        private async Task<string> GetOverAllGradeAsync(int coreGradel, int speciailsmGrade, IndustryPlacementStatus ipStatus)
        {
            await Task.CompletedTask;
            return "A*";
        }
    }
}
