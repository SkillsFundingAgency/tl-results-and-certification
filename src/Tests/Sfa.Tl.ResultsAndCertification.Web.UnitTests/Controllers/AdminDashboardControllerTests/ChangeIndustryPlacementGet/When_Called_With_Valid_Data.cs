using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId).Returns(ViewModel.AdminIpCompletion);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            AssertViewResult();
        }
    }
}
