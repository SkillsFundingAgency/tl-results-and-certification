using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationNotFound
{
    public class When_TempData_Found : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<UlnNotFoundViewModel>(Arg.Any<string>())
                .Returns(new UlnNotFoundViewModel { Uln = Uln, BackLinkRouteName = RouteConstants.SearchRegistration });
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UlnNotFoundViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(Uln);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchRegistration);
            model.BackLink.RouteAttributes.Should().BeNull();
        }
    }
}
