using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommReviewChangesPost
{
    public class When_Process_Fails : TestSetup
    {
        private AdminOpenSpecialismRommReviewChangesViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            AdminPostResultsLoader.ProcessAdminOpenSpecialismRommAsync(_viewModel).Returns(false);
        }

        public async override Task When()
        {
            _result = await Controller.AdminOpenSpecialismRommReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}