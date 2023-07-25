using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
{
    public class When_IndustryPlacements_Null : TestSetup
    {
        public override void Given()
        {
            Source = null;
        }

        [Fact]
        public void Then_Return_NotSpecified()
        {
            Result.Should().Be(IndustryPlacementStatus.NotSpecified.ToString());
        }
    }
}