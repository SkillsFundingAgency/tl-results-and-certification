using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult.PathwayResultConverter
{
    public class When_Assesments_Null : TestSetup
    {
        public override void Given()
        {
            Source = new TqRegistrationPathway
            {
                TqPathwayAssessments = null
            };
        }

        [Fact]
        public void Then_Return_Null()
        {
            Result.Should().BeNull();
        }
    }
}