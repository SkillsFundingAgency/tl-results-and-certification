using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetSearchLearnerFilters
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private SearchLearnerFilters expectedApiResult;

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetSearchLearnerFiltersAsync(Arg.Any<long>())
                .Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
