using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.SearchLearnerDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private readonly PagedResponse<SearchLearnerDetail> _expectedApiResult;

        public override void Given()
        {
            InternalApiClient.SearchLearnerDetailsAsync(Arg.Any<SearchLearnerRequest>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}

