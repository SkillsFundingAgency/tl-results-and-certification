using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqPathwayAssessmentBuilder
    {
        public TqPathwayAssessment Build(TqRegistrationPathway tqRegistrationPathway = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            var assessmentSeries = new AssessmentSeriesBuilder().Build();
            return new TqPathwayAssessment
            {
                TqRegistrationPathwayId = tqRegistrationPathway.Id,
                TqRegistrationPathway = tqRegistrationPathway,
                AssessmentSeriesId = assessmentSeries.Id,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TqPathwayAssessment> BuildList(TqRegistrationPathway tqRegistrationPathway = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            var assessmentSeries = new AssessmentSeriesBuilder().BuildList();

            var tqPathwayAssessments = new List<TqPathwayAssessment> {
                new TqPathwayAssessment
                {
                    TqRegistrationPathwayId = tqRegistrationPathway.Id,
                    TqRegistrationPathway = tqRegistrationPathway,
                    AssessmentSeriesId = assessmentSeries[0].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new TqPathwayAssessment
                {
                    TqRegistrationPathwayId = tqRegistrationPathway.Id,
                    TqRegistrationPathway = tqRegistrationPathway,
                    AssessmentSeriesId = assessmentSeries[1].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,                    
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return tqPathwayAssessments;
        }
    }
}