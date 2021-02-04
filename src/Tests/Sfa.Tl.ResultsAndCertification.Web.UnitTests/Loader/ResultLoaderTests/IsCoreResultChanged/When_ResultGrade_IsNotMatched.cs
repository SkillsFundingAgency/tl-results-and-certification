using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.IsCoreResultChanged
{
    public class When_ResultGrade_IsNotMatched : TestSetup
    {
        public override void Given()
        {
            ViewModel.ResultId = 11;
            expectedApiResultDetails = new ResultDetails { PathwayResultId = ViewModel.ResultId, PathwayResultCode = "Test" };
            InternalApiClient.GetResultDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
