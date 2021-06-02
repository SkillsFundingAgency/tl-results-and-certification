using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.FindSoaLearnerRecord
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.StatementOfAchievement.FindSoaLearnerRecord expectedApiResult;

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.FindSoaLearnerRecordAsync(Arg.Any<long>(), Arg.Any<long>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
