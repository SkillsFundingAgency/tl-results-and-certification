using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkResults
{
    public class ResultsBuilder
    {
        public IList<ResultRecordResponse> BuildValidList() => new List<ResultRecordResponse>
        {
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 1,
                PathwayComponentGradeLookupId = 1,
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 2,
                PathwayComponentGradeLookupId = 1,

                SpecialismResults = new Dictionary<int, int?> { { 11, null} }

            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 3,
                PathwayComponentGradeLookupId = 2,

                SpecialismResults = new Dictionary<int, int?> { { 22, 222} }
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 4,
                PathwayComponentGradeLookupId = 1,

                SpecialismResults = new Dictionary<int, int?> { { 33, null}, { 44, 444 } }
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 5,
                PathwayComponentGradeLookupId = 3,

                SpecialismResults = new Dictionary<int, int?> { { 55, 555}, { 66, null } }

            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = null,
                PathwayComponentGradeLookupId = 4,

                SpecialismResults = new Dictionary<int, int?> { { 77, null}, { 88, null } }
            }
        };
    }
}
