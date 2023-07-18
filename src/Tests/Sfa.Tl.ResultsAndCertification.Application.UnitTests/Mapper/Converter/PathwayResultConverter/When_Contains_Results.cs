using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResultConverter
{
    public class When_Contains_Results : TestSetup
    {
        private readonly TqPathwayResult _aPlusResult = APlusResult;

        public override void Given()
        {
            Source = CreateTqRegistrationPathway(BResult, APlusResult, AResult);
        }

        [Fact]
        public void Then_Return_Highest()
        {
            Result.Should().BeEquivalentTo(_aPlusResult);
        }
    }
}