using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetPrintRequestSnapshot
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private SoaLearnerRecordDetails expectedApiResult = null;

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetSoaLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
