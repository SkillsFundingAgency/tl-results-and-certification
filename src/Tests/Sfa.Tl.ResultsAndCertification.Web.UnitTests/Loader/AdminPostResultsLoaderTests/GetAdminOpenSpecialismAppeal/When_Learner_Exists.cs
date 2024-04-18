using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenSpecialismAppeal
{
    public class When_Learner_Exists : TestSetup
    {
        private AdminLearnerRecord _apiResult;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecordWithSpecialismAssessment(RegistrationPathwayId, SpecialismAssessmentId, RegistrationPathwayStatus.Active, "Pass", "SCG3");
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            AssertResult(_apiResult);

            Result.IsValid.Should().BeTrue();
            Result.ErrorMessage.Should().BeEmpty();
        }
    }
}