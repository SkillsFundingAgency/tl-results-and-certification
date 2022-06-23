using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
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

            return previousAssessment.Year; // TODO: this must be a ResultCalculationYear
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            var resultCalculationYear = await GetResultCalculationYearOfAsync(runDate);
            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYear);
        }

        public async Task<bool> CalculateOverallResultsAsync(DateTime runDate)
        {
            var learners = await GetLearnersForOverallGradeCalculationAsync(runDate);
            return true;
        }
    }
}
