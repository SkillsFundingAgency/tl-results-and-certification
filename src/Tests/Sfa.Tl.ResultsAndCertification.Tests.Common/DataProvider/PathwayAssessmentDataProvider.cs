using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class PathwayAssessmentDataProvider
    {
        public static TqPathwayAssessment CreateTqPathwayAssessment(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var pathwayAssessment = new TqPathwayAssessmentBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(pathwayAssessment);
            }
            return pathwayAssessment;
        }

        public static TqPathwayAssessment CreateTqPathwayAssessment(ResultsAndCertificationDbContext _dbContext, TqPathwayAssessment pathwayAssessment, bool addToDbContext = true)
        {
            if (pathwayAssessment == null)
            {
                pathwayAssessment = new TqPathwayAssessmentBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(pathwayAssessment);
            }
            return pathwayAssessment;
        }

        public static TqPathwayAssessment CreateTqPathwayAssessment(ResultsAndCertificationDbContext _dbContext, int tqRegistrationPathwayId, int assessmentSeriesId, DateTime startDate, bool addToDbContext = true)
        {
            var pathwayAssessment = new TqPathwayAssessment
            {
                TqRegistrationPathwayId = tqRegistrationPathwayId,
                AssessmentSeriesId = assessmentSeriesId,
                StartDate = startDate,
                IsOptedin = true,
                IsBulkUpload = false
            };

            if (addToDbContext)
            {
                _dbContext.Add(pathwayAssessment);
            }
            return pathwayAssessment;
        }
    }
}
