using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqPathwayResultBuilder
    {
        public TqPathwayResult Build(TqPathwayAssessment tqPathwayAssessment = null, TlLookup tlLookupPathwayComponentGrade = null, bool isBulkUpload = true)
        {
            tqPathwayAssessment ??= new TqPathwayAssessmentBuilder().Build();
            tlLookupPathwayComponentGrade ??= new TlLookupBuilder().Build();
            return new TqPathwayResult
            {
                TqPathwayAssessmentId = tqPathwayAssessment.Id,
                TqPathwayAssessment = tqPathwayAssessment,
                TlLookupId = tlLookupPathwayComponentGrade.Id,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = isBulkUpload,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TqPathwayResult> BuildList(TqPathwayAssessment tqPathwayAssessment = null, bool isBulkUpload = true)
        {
            tqPathwayAssessment ??= new TqPathwayAssessmentBuilder().Build();
            var tlLookupPathwayComponentGrades = new TlLookupBuilder().BuildList();

            var TqPathwayResults = new List<TqPathwayResult> {
                new TqPathwayResult
                {
                    TqPathwayAssessmentId = tqPathwayAssessment.Id,
                    TqPathwayAssessment = tqPathwayAssessment,
                    TlLookupId = tlLookupPathwayComponentGrades[0].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new TqPathwayResult
                {
                    TqPathwayAssessmentId = tqPathwayAssessment.Id,
                    TqPathwayAssessment = tqPathwayAssessment,
                    TlLookupId = tlLookupPathwayComponentGrades[1].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return TqPathwayResults;
        }
    }
}
