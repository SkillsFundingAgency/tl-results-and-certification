using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError(nameof(ReviewChangeUlnViewModel.ChangeReason), "Enter the reason why this change is needed");

            Model = Learner();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<ViewResult>().Which;
            result.Model.Should().BeSameAs(Model);
            await AdminDashboardLoader.DidNotReceive().ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>());
        }
    }
}
