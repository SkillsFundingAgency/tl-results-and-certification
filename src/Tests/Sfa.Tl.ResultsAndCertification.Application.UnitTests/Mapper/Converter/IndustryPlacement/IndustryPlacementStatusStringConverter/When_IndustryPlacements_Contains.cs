using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacement.IndustryPlacementStatusStringConverter
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
            TqRegistrationPathway = new TqRegistrationPathway
            {
                Id = TqRegistrationPathwayId,
                IndustryPlacements = new[] { _industryPlacement }
            };
        }

        [Fact]
        public void Then_Return_IndustryPlacement_Status()
        {
            Result.Should().Be(_industryPlacement.Status.ToString());
        }

        public override Task When()
        {
            Result = Converter.Convert(TqRegistrationPathway, null);
            return Task.CompletedTask;
        }
    }
}