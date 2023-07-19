using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
{
    public class When_IndustryPlacements_Contains : TestSetup
    {
        private readonly Domain.Models.IndustryPlacement _industryPlacement = new()
        {
            TqRegistrationPathwayId = 1,
            Status = IndustryPlacementStatus.Completed
        };

        public override void Given()
        {
            Source = new[] { _industryPlacement };
        }

        [Fact]
        public void Then_Return_IndustryPlacement_Status()
        {
            Result.Should().Be(_industryPlacement.Status.ToString());
        }
    }
}