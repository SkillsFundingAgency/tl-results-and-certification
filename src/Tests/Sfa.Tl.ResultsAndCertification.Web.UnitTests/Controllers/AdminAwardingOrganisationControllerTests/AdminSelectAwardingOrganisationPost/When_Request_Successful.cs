using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminSelectAwardingOrganisationPost
{
    public class When_Request_Successful : AdminAwardingOrganisationControllerBaseTest
    {
        private const long SelectedAwardingOrganisationUkprn = 10022490;

        private readonly AdminSelectAwardingOrganisationViewModel _viewModel = new()
        {
            SelectedAwardingOrganisationUkprn = SelectedAwardingOrganisationUkprn,
            AwardingOrganisations = GetAwardingOrganisations()
        };

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminSelectAwardingOrganisationAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, _viewModel, Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminDownloadResultsRommsByAwardingOrganisation,
                (Constants.AwardingOrganisationUkprn, SelectedAwardingOrganisationUkprn));
        }
    }
}