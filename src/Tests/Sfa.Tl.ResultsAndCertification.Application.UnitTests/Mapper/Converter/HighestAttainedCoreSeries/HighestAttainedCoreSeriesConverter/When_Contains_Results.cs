using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedCoreSeries.HighestAttainedCoreSeriesConverter
{
    public class When_Contains_Results : TestSetup
    {
        public override void Given()
        {
            var assessmentWithBestResult = CreateAssessment(Summer2021, APlusResult);
            var assessmentWithLowerResult = CreateAssessment(Summer2022, BResult);
            var assessmentWithAnotherResult = CreateAssessment(Autumn2021, AResult);

            Source = new List<TqPathwayAssessment>
            {
                assessmentWithLowerResult,
                assessmentWithBestResult,
                assessmentWithAnotherResult
            };
        }

        [Fact]
        public void Then_Return_Series_Name_Of_Best_Result()
        {
            Result.Should().Be(Summer2021.Name);
        }
    }
}