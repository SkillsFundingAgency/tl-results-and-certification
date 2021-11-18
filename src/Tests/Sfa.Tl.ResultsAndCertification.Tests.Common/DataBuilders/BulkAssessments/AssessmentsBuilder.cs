using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkAssessments
{
    public class AssessmentsBuilder
    {
        public IList<AssessmentRecordResponse> BuildValidList() => new List<AssessmentRecordResponse>
        {
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = 1,
                PathwayAssessmentSeriesId = 1,
                TqRegistrationSpecialismIds = 2,
                SpecialismAssessmentSeriesId = 2
            },
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = 2,
                PathwayAssessmentSeriesId = 2,
                TqRegistrationSpecialismIds = 3,
                SpecialismAssessmentSeriesId = 2
            },
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = 3,
                PathwayAssessmentSeriesId = 4,
                TqRegistrationSpecialismIds = 5,
                SpecialismAssessmentSeriesId = 1
            },
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = null,
                PathwayAssessmentSeriesId = 1,
                TqRegistrationSpecialismIds = 7,
                SpecialismAssessmentSeriesId = 1
            },
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = 4,
                PathwayAssessmentSeriesId = 4,
                TqRegistrationSpecialismIds = null,
                SpecialismAssessmentSeriesId = 1
            },
            new AssessmentRecordResponse
            {
                TqRegistrationPathwayId = null,
                PathwayAssessmentSeriesId = 4,
                TqRegistrationSpecialismIds = null,
                SpecialismAssessmentSeriesId = 1
            }
        };      
    }
}
