using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedSpecialismSeries
{
    public abstract class HighestAttainedSpecialismSeriesBaseTest<TConverter, TResult> : ConverterBaseTest<TConverter, IEnumerable<TqRegistrationSpecialism>, TResult>
        where TConverter : IValueConverter<IEnumerable<TqRegistrationSpecialism>, TResult>, new()
    {
        protected AssessmentSeries Summer2021 = new() { Name = "Summer 2021", Year = 2021, ComponentType = ComponentType.Specialism };
        protected AssessmentSeries Summer2022 = new() { Name = "Summer 2022", Year = 2022, ComponentType = ComponentType.Specialism };
        protected AssessmentSeries Summer2023 = new() { Name = "Summer 2023", Year = 2023, ComponentType = ComponentType.Specialism };

        protected TqSpecialismResult DistinctionResult = CreateResult("SCG1", "Distinction", 1);
        protected TqSpecialismResult MeritResult = CreateResult("SCG2", "Merit", 2);
        protected TqSpecialismResult PassResult = CreateResult("SCG3", "Pass", 3);

        private static TqSpecialismResult CreateResult(string code, string value, int sortOrder)
        {
            return new TqSpecialismResult
            {
                TlLookup = new()
                {
                    Category = "SpecialismComponentGrade",
                    Code = code,
                    Value = value,
                    SortOrder = sortOrder
                }
            };
        }

        protected TqRegistrationSpecialism CreateSpecialism(AssessmentSeries series, params TqSpecialismResult[] results)
        {
            var specialism = new TqRegistrationSpecialism
            {
                TqSpecialismAssessments = new List<TqSpecialismAssessment>()
            };

            var assessment = new TqSpecialismAssessment
            {
                AssessmentSeries = series,
                TqRegistrationSpecialism = specialism,
                TqSpecialismResults = new List<TqSpecialismResult>()
            };
            ((List<TqSpecialismAssessment>)specialism.TqSpecialismAssessments).Add(assessment);

            foreach (var result in results)
            {
                result.TqSpecialismAssessment = assessment;
                ((List<TqSpecialismResult>)assessment.TqSpecialismResults).Add(result);
            }

            return specialism;
        }
    }
}