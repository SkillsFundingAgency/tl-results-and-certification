using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetPrintRequestSnapshot
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private PrintRequestSnapshot _expectedApiResult = null;

        public override void Given()
        {
            _expectedApiResult = null;
            InternalApiClient.GetPrintRequestSnapshotAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
