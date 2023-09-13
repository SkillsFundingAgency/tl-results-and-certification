using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToCoreRommExtractData
{
    public class When_Contains_Multiple_Core_Grades : TestSetup
    {

        public override void Given()
        {
            TqPathwayResult[] results = new[] 
            { 
                CreateTqPathwayResult(1, "C", false),
                CreateTqPathwayResult(2, "B", true),
            };

            SetSourceResults(results);
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentCoreGrade.Should().Be("B");
            Destination.RommOpenedTimeStamp.Should().NotHaveValue();
            Destination.RommGrade.Should().BeEmpty();
            Destination.AppealOpenedTimeStamp.Should().NotHaveValue();
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}