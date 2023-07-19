using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusConverter
{
    public class When_IndustryPlacements_Contains : TestSetup
    {
        private const int TqRegistrationPathwayId = 1;

        private readonly Domain.Models.IndustryPlacement _industryPlacement = new()
        {
            TqRegistrationPathwayId = TqRegistrationPathwayId,
            Status = IndustryPlacementStatus.Completed
        };

        public override void Given()
        {
            Source = new[] { _industryPlacement };
        }

        [Fact]
        public void Then_Return_IndustryPlacement_Status()
        {
            Result.Should().Be(_industryPlacement.Status);
        }
    }
}