using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminCoreRommOutcomeReviewChangesPost
{
    public class When_Process_Fails : TestSetup
    {
        private AdminReviewChangesRommOutcomeCoreViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            AdminPostResultsLoader.ProcessAdminReviewChangesRommOutcomeCoreAsync(_viewModel).Returns(false);
        }

        public async override Task When()
        {
            _result = await Controller.AdminReviewChangesRommOutcomeCoreAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}