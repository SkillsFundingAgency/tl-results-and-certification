using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewCoreAppealChangesPost
{
    public class When_Process_Fails : TestSetup
    {
        private AdminAppealCoreReviewChangesViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            AdminPostResultsLoader.ProcessAdminOpenCoreAppealAsync(_viewModel).Returns(false);
        }

        public async override Task When()
        {
            _result = await Controller.AdminReviewCoreAppealChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}