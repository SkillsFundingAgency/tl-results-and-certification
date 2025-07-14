using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeMathsStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(RegistrationPathwayId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            AssertViewResult();
        }
    }
}
