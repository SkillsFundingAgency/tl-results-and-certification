using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResultConverter
{
    public class When_Contains_QPendingResult : TestSetup
    {
        private readonly TqPathwayResult _qPendingResult = QPendingResult;

        public override void Given()
        {
            Source = CreateTqRegistrationPathway(BResult, _qPendingResult);
        }

        [Fact]
        public void Then_Return_QPendingResult()
        {
            Result.Should().Be(_qPendingResult);
        }
    }
}