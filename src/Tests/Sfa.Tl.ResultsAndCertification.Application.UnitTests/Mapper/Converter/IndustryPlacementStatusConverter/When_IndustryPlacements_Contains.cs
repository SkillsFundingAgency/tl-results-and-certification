using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacementStatusConverter
{
    public class When_IndustryPlacements_Contains : TestSetup
    {
        private const int TqRegistrationPathwayId = 1;

        private readonly IndustryPlacement _industryPlacement = new()
        {
            TqRegistrationPathwayId = TqRegistrationPathwayId,
            Status = IndustryPlacementStatus.Completed
        };

        public override void Given()
        {
            Source = new TqRegistrationPathway
            {
                Id = TqRegistrationPathwayId,
                IndustryPlacements = new[] { _industryPlacement }
            };
        }

        [Fact]
        public void Then_Return_IndustryPlacement_Status()
        {
            Result.Should().Be(_industryPlacement.Status);
        }
    }
}