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
                PathwayComponentGradeLookupId = 1                
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 2,
                PathwayComponentGradeLookupId = 1
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 3,
                PathwayComponentGradeLookupId = 2
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 4,
                PathwayComponentGradeLookupId = 1
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = 5,
                PathwayComponentGradeLookupId = 3
            },
            new ResultRecordResponse
            {
                TqPathwayAssessmentId = null,
                PathwayComponentGradeLookupId = 4
            }
        };
    }
}
