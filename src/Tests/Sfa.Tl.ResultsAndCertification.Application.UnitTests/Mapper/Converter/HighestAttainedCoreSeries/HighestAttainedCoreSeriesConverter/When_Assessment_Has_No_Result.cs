using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedCoreSeries.HighestAttainedCoreSeriesConverter
{
    public class When_Assessment_Has_No_Result : TestSetup
    {
        public override void Given()
        {
            var assessmentWithNoResult = CreateAssessment(Summer2022);

            Source = new List<TqPathwayAssessment>
            {
                assessmentWithNoResult
            };
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}