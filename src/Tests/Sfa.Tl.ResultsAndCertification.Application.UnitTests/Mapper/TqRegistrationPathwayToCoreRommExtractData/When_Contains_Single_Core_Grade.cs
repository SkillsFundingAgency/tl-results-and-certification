using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToCoreRommExtractData
{
    public class When_Contains_Single_Core_Grade : TestSetup
    {
        private readonly string _grade = "A"; 

        public override void Given()
        {
            TqPathwayResult result = CreateTqPathwayResult(1, _grade);
            SetSourceResults(new[] { result });
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentCoreGrade.Should().Be(_grade);
            Destination.RommOpenedTimeStamp.Should().NotHaveValue();
            Destination.RommGrade.Should().BeEmpty();
            Destination.AppealOpenedTimeStamp.Should().NotHaveValue();
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}