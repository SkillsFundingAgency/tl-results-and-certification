using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult.PathwayResultConverter
{
    public class When_Contains_Results : TestSetup
    {
        public override void Given()
        {
            Source = CreateTqRegistrationPathway(BResult, APlusResult, AResult);
        }

        [Fact]
        public void Then_Return_Highest()
        {
            Result.Should().Be(APlusResult);
        }
    }
}