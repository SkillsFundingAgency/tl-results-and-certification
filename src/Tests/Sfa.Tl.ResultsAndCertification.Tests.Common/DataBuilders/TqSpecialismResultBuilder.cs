using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqSpecialismResultBuilder
    {
        public TqSpecialismResult Build(TqSpecialismAssessment tqSpecialismAssessment = null, TlLookup tlLookupPathwayComponentGrade = null, bool isBulkUpload = true)
        {
            tqSpecialismAssessment ??= new TqSpecialismAssessmentBuilder().Build();
            tlLookupPathwayComponentGrade ??= new TlLookupBuilder().BuildSpecialismResult();
            return new TqSpecialismResult
            {
                TqSpecialismAssessmentId = tqSpecialismAssessment.Id,
                TqSpecialismAssessment = tqSpecialismAssessment,
                TlLookupId = tlLookupPathwayComponentGrade.Id,
                TlLookup = tlLookupPathwayComponentGrade,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = isBulkUpload,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TqSpecialismResult> BuildList(TqSpecialismAssessment tqSpecialismAssessment = null, bool isBulkUpload = true)
        {
            tqSpecialismAssessment ??= new TqSpecialismAssessmentBuilder().Build();
            var tlLookupSpecialismComponentGrades = new TlLookupBuilder().BuildSpecialismResultList();

            var tqSpecialismResults = new List<TqSpecialismResult> {
                new TqSpecialismResult
                {
                    TqSpecialismAssessmentId = tqSpecialismAssessment.Id,
                    TqSpecialismAssessment = tqSpecialismAssessment,
                    TlLookupId = tlLookupSpecialismComponentGrades[0].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new TqSpecialismResult
                {
                    TqSpecialismAssessmentId = tqSpecialismAssessment.Id,
                    TqSpecialismAssessment = tqSpecialismAssessment,
                    TlLookupId = tlLookupSpecialismComponentGrades[1].Id,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return tqSpecialismResults;
        }
    }
}
