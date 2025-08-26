using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.HighestAttainedCoreSeries
{
    public class HighestAttainedCoreSeriesConverter : HighestAttainedCoreSeriesConverterBase, IValueConverter<IEnumerable<TqPathwayAssessment>, string>
    {
        public string Convert(IEnumerable<TqPathwayAssessment> assessments, ResolutionContext context)
        {
            TqPathwayResult tqPathwayResult = Convert(assessments);
            string result = tqPathwayResult?.TqPathwayAssessment?.AssessmentSeries?.Name;

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }
    }
}
