using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqSpecialismAssessmentBuilder
    {
        public TqSpecialismAssessment Build(TqRegistrationSpecialism tqRegistrationSpecialism = null, AssessmentSeries assessmentSeries = null, bool isBulkUpload = true)
        {
            tqRegistrationSpecialism ??= new TqRegistrationSpecialismBuilder().Build();
            assessmentSeries ??= new AssessmentSeriesBuilder().Build();
            return new TqSpecialismAssessment
            {
                TqRegistrationSpecialismId = tqRegistrationSpecialism.Id,
                TqRegistrationSpecialism = tqRegistrationSpecialism,
                AssessmentSeriesId = assessmentSeries.Id,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = isBulkUpload,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TqSpecialismAssessment> BuildList(TqRegistrationSpecialism tqRegistrationSpecialism = null, bool isBulkUpload = true)
        {
            tqRegistrationSpecialism ??= new TqRegistrationSpecialismBuilder().Build();
            var assessmentSeries = new AssessmentSeriesBuilder().BuildList();

            var tqSpecialismAssessments = new List<TqSpecialismAssessment> {
                new TqSpecialismAssessment
                {
                    TqRegistrationSpecialismId = tqRegistrationSpecialism.Id,
                    TqRegistrationSpecialism = tqRegistrationSpecialism,
                    AssessmentSeriesId = assessmentSeries[0].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new TqSpecialismAssessment
                {
                    TqRegistrationSpecialismId = tqRegistrationSpecialism.Id,
                    TqRegistrationSpecialism = tqRegistrationSpecialism,
                    AssessmentSeriesId = assessmentSeries[1].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return tqSpecialismAssessments;
        }
    }
}
