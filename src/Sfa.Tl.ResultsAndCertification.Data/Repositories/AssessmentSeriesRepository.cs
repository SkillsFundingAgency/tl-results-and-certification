using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AssessmentSeriesRepository : IAssessmentSeriesRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AssessmentSeriesRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssessmentSeries> GetPreviousAssessmentSeriesAsync(DateTime utcNow)
        {
            List<AssessmentSeries> coreAssessmentSeries = await _dbContext.AssessmentSeries.Where(a => a.ComponentType == ComponentType.Core).ToListAsync();

            AssessmentSeries currentAssessmentSeries = coreAssessmentSeries.FirstOrDefault(a => utcNow.Date >= a.StartDate && utcNow.Date <= a.EndDate);

            if (currentAssessmentSeries == null)
                return null;

            DateTime dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            AssessmentSeries previousCoreAssessmentSeries = coreAssessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            if (previousCoreAssessmentSeries == null || !previousCoreAssessmentSeries.ResultCalculationYear.HasValue || !previousCoreAssessmentSeries.ResultPublishDate.HasValue)
                return null;

            return previousCoreAssessmentSeries;
        }
    }
}