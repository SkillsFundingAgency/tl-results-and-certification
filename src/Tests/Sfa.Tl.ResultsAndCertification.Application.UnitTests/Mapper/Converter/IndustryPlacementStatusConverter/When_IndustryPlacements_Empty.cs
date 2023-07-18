using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacementStatusConverter
{
    public class When_IndustryPlacements_Empty : TestSetup
    {
        public override void Given()
        {
            Source = new TqRegistrationPathway();
        }

        [Fact]
        public void Then_Return_NotSpecified()
        {
            Result.Should().Be(IndustryPlacementStatus.NotSpecified);
        }
    }
}