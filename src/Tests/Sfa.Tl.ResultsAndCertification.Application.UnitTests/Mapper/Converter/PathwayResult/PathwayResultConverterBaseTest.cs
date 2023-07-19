using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult
{
    public abstract class PathwayResultConverterBaseTest<TConverter, TResult> : ConverterBaseTest<TConverter, IEnumerable<TqPathwayAssessment>, TResult>
        where TConverter : IValueConverter<IEnumerable<TqPathwayAssessment>, TResult>, new()
    {
        protected TqPathwayResult APlusResult = CreateResult("PCG1", "A*", 1);
        protected TqPathwayResult AResult = CreateResult("PCG2", "A", 2);
        protected TqPathwayResult BResult = CreateResult("PCG3", "B", 3);
        protected TqPathwayResult QPendingResult = CreateResult(Constants.PathwayComponentGradeQpendingResultCode, "Q - pending result", 8);

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

        protected IEnumerable<TqPathwayAssessment> CreateTqRegistrationPathway(TqPathwayResult result, params TqPathwayResult[] extraResults)
        {
            var results = new List<TqPathwayResult> { result };

            if (extraResults != null)
            {
                results.AddRange(extraResults);
            }

            return new[]
            {
                new TqPathwayAssessment
                {
                    TqPathwayResults = results
                }
            };
        }
    }
}