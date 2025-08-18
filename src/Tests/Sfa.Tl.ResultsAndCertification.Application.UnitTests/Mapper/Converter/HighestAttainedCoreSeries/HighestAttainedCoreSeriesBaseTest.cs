using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedCoreSeries
{
    public abstract class HighestAttainedCoreSeriesBaseTest<TConverter, TResult> : ConverterBaseTest<TConverter, IEnumerable<TqPathwayAssessment>, TResult>
        where TConverter : IValueConverter<IEnumerable<TqPathwayAssessment>, TResult>, new()
    {
        protected AssessmentSeries Summer2021 = new() { Name = "Summer 2021", Year = 2021, ComponentType = ComponentType.Core };
        protected AssessmentSeries Autumn2021 = new() { Name = "Autumn 2021", Year = 2021, ComponentType = ComponentType.Core };
        protected AssessmentSeries Summer2022 = new() { Name = "Summer 2022", Year = 2022, ComponentType = ComponentType.Core };

        protected TqPathwayResult APlusResult = CreateResult("PCG1", "A*", 1);
        protected TqPathwayResult AResult = CreateResult("PCG2", "A", 2);
        protected TqPathwayResult BResult = CreateResult("PCG3", "B", 3);

        private static TqPathwayResult CreateResult(string code, string value, int sortOrder)
        {
            return new TqPathwayResult
            {
                TlLookup = new()
                {
                    Category = "PathwayComponentGrade",
                    Code = code,
                    Value = value,
                    SortOrder = sortOrder
                }
            };
        }

        // Corrected helper method
        protected TqPathwayAssessment CreateAssessment(AssessmentSeries series, params TqPathwayResult[] results)
        {
            var assessment = new TqPathwayAssessment
            {
                AssessmentSeries = series,
                TqPathwayResults = new List<TqPathwayResult>()
            };

            foreach (var result in results)
            {
                result.TqPathwayAssessment = assessment;
                ((List<TqPathwayResult>)assessment.TqPathwayResults).Add(result);
            }
            
            return assessment;
        }
    }
}