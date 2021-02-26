using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.IsCoreResultChanged
{
    public class When_ResultGrade_IsMatched : TestSetup
    {
        public override void Given()
        {
            ViewModel.ResultId = 11;
            expectedApiResultDetails = new ResultDetails { PathwayResultId = ViewModel.ResultId, PathwayResultCode = ViewModel.SelectedGradeCode };
            InternalApiClient.GetResultDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_False()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
