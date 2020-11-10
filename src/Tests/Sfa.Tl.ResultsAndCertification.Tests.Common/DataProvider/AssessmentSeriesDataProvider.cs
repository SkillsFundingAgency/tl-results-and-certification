using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class AssessmentSeriesDataProvider
    {
        public static AssessmentSeries CreateAssessmentSeries(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var assessmentSeries = new AssessmentSeriesBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(assessmentSeries);
            }
            return assessmentSeries;
        }

        public static AssessmentSeries CreateAssessmentSeries(ResultsAndCertificationDbContext _dbContext, AssessmentSeries assessmentSeries, bool addToDbContext = true)
        {
            if (assessmentSeries == null)
            {
                assessmentSeries = new AssessmentSeriesBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(assessmentSeries);
            }
            return assessmentSeries;
        }

        public static AssessmentSeries CreateAssessmentSeries(ResultsAndCertificationDbContext _dbContext, string name, string description, int year, DateTime endDate, bool addToDbContext = true)
        {
            var assessmentSeries = new AssessmentSeries
            {
                Name = name,
                Description = description,
                Year = year,
                EndDate = endDate,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(assessmentSeries);
            }
            return assessmentSeries;
        }

        public static IList<AssessmentSeries> CreateAssessmentSeriesList(ResultsAndCertificationDbContext _dbContext, IList<AssessmentSeries> assessmentSeries, bool addToDbContext = true)
        {
            if (assessmentSeries == null)
                assessmentSeries = new AssessmentSeriesBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(assessmentSeries);
            }
            return assessmentSeries;
        }
    }
}
