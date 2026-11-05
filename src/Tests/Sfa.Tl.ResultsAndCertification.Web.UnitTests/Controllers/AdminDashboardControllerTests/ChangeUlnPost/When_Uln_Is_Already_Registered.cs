using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnPost
{
    public class When_Uln_Is_Already_Registered : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(3).Returns(Entry());
            AdminDashboardLoader.IsLearnerUlnAvailableAsync(9999999991).Returns(false);
            Model = Entry();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType<ViewResult>();
            Controller.ModelState["UlnTo"].Errors[0].ErrorMessage.Should().Be("ULN is already registered");
        }
    }
}
