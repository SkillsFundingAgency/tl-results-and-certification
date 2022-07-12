using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DataExportService : IDataExportService
    {
        public readonly IDataExportRepository _dataExportRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;
        private readonly IRepository<OverallResult> _overallResultsRepository;
        private readonly IMapper _mapper;

        public DataExportService(IDataExportRepository dataExportRepository,
            IRepository<AssessmentSeries> assessmentSeriesRepository,
            IRepository<OverallResult> tqRegistrationPathway, 
            IMapper mapper)
        {
            _dataExportRepository = dataExportRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _overallResultsRepository = tqRegistrationPathway;
            _mapper = mapper;
        }

        public async Task<IList<RegistrationsExport>> GetDataExportRegistrationsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportRegistrationsAsync(aoUkprn);
        }

        public async Task<IList<CoreAssessmentsExport>> GetDataExportCoreAssessmentsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportCoreAssessmentsAsync(aoUkprn);
        }

        public async Task<IList<SpecialismAssessmentsExport>> GetDataExportSpecialismAssessmentsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportSpecialismAssessmentsAsync(aoUkprn);
        }

        public async Task<IList<CoreResultsExport>> GetDataExportCoreResultsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportCoreResultsAsync(aoUkprn);
        }

        public async Task<IList<SpecialismResultsExport>> GetDataExportSpecialismResultsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportSpecialismResultsAsync(aoUkprn);
        }

        public async Task<IList<DownloadOverallResultsData>> DownloadOverallResultsDataAsync(long providerUkprn)
        {
            // 1. Get the Previous assessment PublishDate
            var resultPublishDate = await GetOverallResultPublishDateAsync();
            if (resultPublishDate == null)
                return new List<DownloadOverallResultsData>();

            // 2. Get OverallResults on above PublishDate if date reached
            var overallResults = await _overallResultsRepository.GetManyAsync(x =>
                                                x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                                x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == providerUkprn &&
                                                x.PublishDate == resultPublishDate && DateTime.Today >= resultPublishDate,
                                                incl => incl.TqRegistrationPathway.TqRegistrationProfile)
                                        .ToListAsync();

            var overallResultsData = _mapper.Map<IList<DownloadOverallResultsData>>(overallResults);
            return overallResultsData;
        }

        private async Task<DateTime?> GetOverallResultPublishDateAsync()
        {
            var runDate = DateTime.UtcNow;
            var assessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => runDate >= a.StartDate && runDate <= a.EndDate);
            if (currentAssessmentSeries == null)
                throw new Exception($"There is no AssessmentSeries available for the date {runDate}");

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment?.ResultPublishDate;
        }
    }
}
