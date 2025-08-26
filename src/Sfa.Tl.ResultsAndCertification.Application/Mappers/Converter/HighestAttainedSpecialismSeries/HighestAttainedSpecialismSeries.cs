using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.HighestAttainedSpecialismSeries
{
    public class HighestAttainedSpecialismSeriesConverter : HighestAttainedSpecialismSeriesConverterBase, IValueConverter<IEnumerable<TqRegistrationSpecialism>, string>
    {
        public string Convert(IEnumerable<TqRegistrationSpecialism> specialisms, ResolutionContext context)
        {
            TqSpecialismResult tqSpecialismResult = Convert(specialisms);
            string result = tqSpecialismResult?.TqSpecialismAssessment?.AssessmentSeries?.Name;

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }
    }
}