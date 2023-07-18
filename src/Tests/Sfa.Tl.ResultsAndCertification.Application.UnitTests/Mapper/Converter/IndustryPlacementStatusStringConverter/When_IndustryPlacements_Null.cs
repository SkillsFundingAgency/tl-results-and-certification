using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.IndustryPlacementStatusStringConverter
{
    public class When_IndustryPlacements_Null : TestSetup
    {
        public override void Given()
        {
            TqRegistrationPathway = new TqRegistrationPathway
            {
                IndustryPlacements = null
            };
        }

        [Fact]
        public void Then_Return_NotSpecified()
        {
            Result.Should().Be(IndustryPlacementStatus.NotSpecified.ToString());
        }

        public override Task When()
        {
            Result = Converter.Convert(TqRegistrationPathway, null);
            return Task.CompletedTask;
        }
    }
}