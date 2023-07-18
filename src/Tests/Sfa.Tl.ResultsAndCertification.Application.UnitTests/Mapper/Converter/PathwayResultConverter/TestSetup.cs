using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using PathwayConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResultConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResultConverter
{
    public abstract class TestSetup : ConverterBaseTest<PathwayConverter, TqRegistrationPathway, TqPathwayResult>
    {
        protected static TqPathwayResult APlusResult
            => CreateResult("PCG1", "A*", 1);

        protected static TqPathwayResult AResult
            => CreateResult("PCG2", "A", 2);

        protected static TqPathwayResult BResult
            => CreateResult("PCG3", "B", 3);

        protected static TqPathwayResult QPendingResult
            => CreateResult(Constants.PathwayComponentGradeQpendingResultCode, "Q - pending result", 8);

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

        protected TqRegistrationPathway CreateTqRegistrationPathway(TqPathwayResult result, params TqPathwayResult[] extraResults)
        {
            var results = new List<TqPathwayResult> { result };

            if (extraResults != null)
            {
                results.AddRange(extraResults);
            }

            return new TqRegistrationPathway
            {
                TqPathwayAssessments = new[]
                {
                    new TqPathwayAssessment
                    {
                        TqPathwayResults = results
                    }
                }
            };
        }
    }
}