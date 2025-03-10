using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminSelectAwardingOrganisationPost
{
    public class When_ModelState_Invalid : AdminAwardingOrganisationControllerBaseTest
    {
        private const string ErrorKey = "SelectedAwardingOrganisationId";

        private readonly AdminSelectAwardingOrganisationViewModel _viewModel = new()
        {
            SelectedAwardingOrganisationUkprn = null,
            AwardingOrganisations = GetAwardingOrganisations()
        };

        private IActionResult _result;

        public override void Given()
        {
            Controller.ModelState.AddModelError(ErrorKey, SelectAwardingOrganisation.Validation_Message);
        }

        public async override Task When()
        {
            _result = await Controller.AdminSelectAwardingOrganisationAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminSelectAwardingOrganisationViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(1);
            Controller.ModelState.Should().ContainKey(ErrorKey);
            Controller.ModelState[ErrorKey].Errors.Should().ContainSingle(SelectAwardingOrganisation.Validation_Message);

            var resultViewModel = _result.ShouldBeViewResult<AdminSelectAwardingOrganisationViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}