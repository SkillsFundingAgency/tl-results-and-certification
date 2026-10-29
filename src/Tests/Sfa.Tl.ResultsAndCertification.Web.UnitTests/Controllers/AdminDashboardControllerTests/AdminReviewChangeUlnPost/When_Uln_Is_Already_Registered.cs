using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnPost
{
    public class When_Uln_Is_Already_Registered : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            AdminDashboardLoader.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>()).Returns(false);
            AdminDashboardLoader.IsLearnerUlnAvailableAsync(9999999991).Returns(false);
            Model = Learner();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<ViewResult>().Which;
            result.ViewName.Should().Be("ChangeUln");
            result.Model.Should().BeOfType<AdminChangeUlnViewModel>();
            Controller.ModelState["UlnTo"].Errors[0].ErrorMessage.Should().Be("ULN is already registered");
        }
    }
}
