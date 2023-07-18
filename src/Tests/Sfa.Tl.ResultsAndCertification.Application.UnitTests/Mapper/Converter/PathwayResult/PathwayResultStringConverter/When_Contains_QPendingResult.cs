using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult.PathwayResultStringConverter
{
    public class When_Contains_QPendingResult : TestSetup
    {
        public override void Given()
        {
            Source = CreateTqRegistrationPathway(BResult, QPendingResult);
        }

        [Fact]
        public void Then_Return_QPendingResult()
        {
            Result.Should().Be(QPendingResult.TlLookup.Value);
        }
    }
}