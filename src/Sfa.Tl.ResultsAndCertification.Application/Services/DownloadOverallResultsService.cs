using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DownloadOverallResultsService : IDownloadOverallResultsService
    {
        private readonly IOverallResultRepository _overallResultRepository;
        private readonly IAssessmentSeriesRepository _assessmentSeriesRepository;
        private readonly IMapper _mapper;

        public DownloadOverallResultsService(
            IOverallResultRepository overallResultRepository,
            IAssessmentSeriesRepository assessmentSeriesRepository,
            IMapper mapper)
        {
            _overallResultRepository = overallResultRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _mapper = mapper;
        }

        public Task<IList<DownloadOverallResultsData>> DownloadOverallResultsDataAsync(long providerUkprn, DateTime now)
            => DownloadOverallResultsAsync<DownloadOverallResultsData>(providerUkprn, now);

        public Task<IList<DownloadOverallResultSlipsData>> DownloadOverallResultSlipsDataAsync(long providerUkprn, DateTime now)
            => DownloadOverallResultsAsync<DownloadOverallResultSlipsData>(providerUkprn, now);

        public async Task<DownloadOverallResultSlipsData> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, long profileId)
        {
            var overallResults = await _overallResultRepository.GetLearnerOverallResults(providerUkprn, profileId);
            return _mapper.Map<DownloadOverallResultSlipsData>(overallResults);
        }

        private async Task<IList<T>> DownloadOverallResultsAsync<T>(long providerUkprn, DateTime now)
        {
            DateTime today = now.Date;

            // 1. Get the the result calculation year from previous assessment.
            AssessmentSeries previousAssessment = await GetPreviousAssessmentSeriesAsync(now);

            int? resultCalculationYear = previousAssessment?.ResultCalculationYear;

            if (!resultCalculationYear.HasValue)
                return new List<T>();

            // 2. Get the overall results for learners from the result calculation year, but only if the publish date is today or later.
            var overallResults = await _overallResultRepository.GetOverallResults(providerUkprn, resultCalculationYear.Value, today);
            return _mapper.Map<IList<T>>(overallResults);
        }

        private async Task<AssessmentSeries> GetPreviousAssessmentSeriesAsync(DateTime now)
        {
            AssessmentSeries previousAssessmentSeries = await _assessmentSeriesRepository.GetPreviousAssessmentSeriesAsync(now);
            return previousAssessmentSeries ?? throw new Exception($"There is no previous assessment or result calculation year not available or result publish date not available.");
        }
    }
}