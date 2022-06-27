using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class OverallResultCalculationService : IOverallResultCalculationService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IRepository<OverallGradeLookup> _overallGradeLookupRepository;
        private readonly IOverallResultCalculationRepository _overallGradeCalculationRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;

        public OverallResultCalculationService(
            ResultsAndCertificationConfiguration configuration,
            IRepository<TlLookup> tlLookupRepository,
            IRepository<OverallGradeLookup> overallGradeLookupRepository,
            IOverallResultCalculationRepository overallGradeCalculationRepository,
            IRepository<AssessmentSeries> assessmentSeriesRepository)
        {
            _configuration = configuration;
            _tlLookupRepository = tlLookupRepository;
            _overallGradeLookupRepository = overallGradeLookupRepository;
            _overallGradeCalculationRepository = overallGradeCalculationRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
        }

        public async Task<int> GetResultCalculationYearOfAsync(DateTime runDate)
        {
            var assessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => runDate >= a.StartDate && runDate <= a.EndDate);
            if (currentAssessmentSeries == null)
                throw new Exception($"There is no AssessmentSeries available for the date {runDate}");

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment?.ResultCalculationYear ?? 0;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            var resultCalculationYear = await GetResultCalculationYearOfAsync(runDate);
            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYear - 3, resultCalculationYear);
        }

        public async Task<bool> CalculateOverallResultsAsync(DateTime runDate)
        {
            var learners = await GetLearnersForOverallGradeCalculationAsync(runDate);

            if (learners == null || !learners.Any())
                return true;

            var tasks = new List<Task>();
            var batchSize = _configuration.OverallResultBatchSettings.BatchSize <= 0 ? Constants.OverallResultDefaultBatchSize : _configuration.OverallResultBatchSettings.BatchSize;            
            var batchesToProcess = (int)Math.Ceiling(learners.Count / (decimal)batchSize);

            for (var batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                var leanersToProcess = learners.Skip(batchIndex * batchSize).Take(batchSize);
                tasks.Add(ProcessOverallResults(leanersToProcess));
            }

            await Task.WhenAll(tasks);
            return true;
        }

        private async Task ProcessOverallResults(IEnumerable<TqRegistrationPathway> learners)
        {
            foreach (var learner in learners)
            {
                var coreGrade = await GetHightestCoreGradeAsync(learner);
                var specialismGrade = await GetHightestSpecialismGradeAsync(learner);
                var ipStatus = learner.IndustryPlacements.Any() ? learner.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;

                var overallGrade = await GetOverAllGradeAsync(coreGrade, specialismGrade, ipStatus);

                // Save.
            }
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

        private async Task<List<TlLookup>> GetTlLookupData()
        {
            return await _tlLookupRepository.GetManyAsync().ToListAsync();
        }

        private async Task<List<OverallGradeLookup>> GetOverallGradeLookupData()
        {
            return await _overallGradeLookupRepository.GetManyAsync().ToListAsync();
        }
    }
}
