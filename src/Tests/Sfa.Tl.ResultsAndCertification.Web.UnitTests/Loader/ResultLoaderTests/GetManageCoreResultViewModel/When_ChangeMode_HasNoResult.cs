using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageCoreResultViewModel
{
    public class When_ChangeMode_HasNoResult : TestSetup
    {
        public override void Given()
        {
            IsChangeMode = true;

            expectedApiResultDetails = new Models.Contracts.ResultDetails { PathwayResultId = null };
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.DidNotReceive().GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }
    }
}
