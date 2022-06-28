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

        public async Task<AssessmentSeries> GetResultCalculationAssessmentAsync(DateTime runDate)
        {
            var assessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => runDate >= a.StartDate && runDate <= a.EndDate);
            if (currentAssessmentSeries == null)
                throw new Exception($"There is no AssessmentSeries available for the date {runDate}");

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            // Dev note: This method left to test from api end-point
            var resultCalculationYear = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) - 1;

            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, resultCalculationYear?.ResultCalculationYear ?? 0);
        }

        public async Task<bool> CalculateOverallResultsAsync(DateTime runDate)
        {
            var resultCalculationAssessment = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) - 1;
            var learners = await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, resultCalculationAssessment.ResultCalculationYear ?? 0);

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

        private async Task ProcessOverallResults(IEnumerable<TqRegistrationPathway> learnerPathways)
        {
            foreach (var pathway in learnerPathways)
            {
                var pathwayResult = GetHighestPathwayResult(pathway);
                var specialismResult = GetHighestSpecialismResult(pathway).FirstOrDefault(); // as we are not dealing with couplet specialisms as of now
                var ipStatus = pathway.IndustryPlacements.Any() ? pathway.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;

                var overallGrade = await GetOverAllGrade(pathwayResult?.TlLookupId, specialismResult?.TlLookupId, ipStatus);                
            }

            // Save.
        }

        public TqPathwayResult GetHighestPathwayResult(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqPathwayAssessments.Any())
                return null;

            var pathwayHigherResult = learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return pathwayHigherResult;
        }

        public List<TqSpecialismResult> GetHighestSpecialismResult(TqRegistrationPathway learnerPathway)
        {
            var specialismResults = new List<TqSpecialismResult>();
            foreach (var specialism in learnerPathway.TqRegistrationSpecialisms.Where(specialism => specialism.TqSpecialismAssessments.Any()))
            {
                var specialismHigherResult = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();
                specialismResults.Add(specialismHigherResult);
            }

            return specialismResults;
        }

        private async Task<string> GetOverAllGrade(int? coreGrade, int? speciailsmGrade, IndustryPlacementStatus ipStatus)
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
