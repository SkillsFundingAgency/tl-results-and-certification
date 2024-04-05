using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenSpecialismRomm
{
    public class When_Learner_Result_XNoResult : TestSetup
    {
        private AdminLearnerRecord _apiResult;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecordWithSpecialismAssessment(RegistrationPathwayId, SpecialismAssessmentId, RegistrationPathwayStatus.Active, "X - no result", Constants.SpecialismComponentGradeXNoResultCode);
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

            Result.IsValid.Should().BeFalse();
            Result.ErrorMessage.Should().Be(AdminOpenSpecialismRomm.Validation_No_Result);
        }
    }
}