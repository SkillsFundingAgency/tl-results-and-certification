using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationAssessment
{
    public class When_Assessment_Has_Result : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new AssessmentDetails { PathwayResultId = 99 };
            InternalApiClient.GetAssessmentDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_IsResultExist_IsTrue()
        {
            ActualResult.IsCoreResultExist.Should().BeTrue();
        }
    }
}
