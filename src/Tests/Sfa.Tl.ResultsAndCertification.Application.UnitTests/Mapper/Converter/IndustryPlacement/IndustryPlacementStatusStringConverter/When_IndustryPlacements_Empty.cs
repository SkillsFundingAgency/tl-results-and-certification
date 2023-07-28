using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
{
    public class When_IndustryPlacements_Empty : TestSetup
    {
        public override void Given()
        {
            Source = Enumerable.Empty<Domain.Models.IndustryPlacement>();
        }

        [Fact]
        public void Then_Return_NotSpecified()
        {
            Result.Should().Be(IndustryPlacementStatus.NotSpecified.ToString());
        }
    }
}