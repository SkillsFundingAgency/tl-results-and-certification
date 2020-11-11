using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UlnCannotBeRegisteredGet
{
    public class When_TempData_Found : TestSetup
    {
        private UlnRegistrationNotFoundViewModel expectedViewModel;
        public override void Given() 
        {
            expectedViewModel = new UlnRegistrationNotFoundViewModel();
            CacheService.GetAndRemoveAsync<UlnRegistrationNotFoundViewModel>(Arg.Any<string>()).Returns(expectedViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UlnRegistrationNotFoundViewModel;

            model.Should().NotBeNull();
            model.RegistrationProfileId.Should().Be(expectedViewModel.RegistrationProfileId);
            model.IsActive.Should().Be(expectedViewModel.IsActive);
            model.IsRegisteredWithOtherAo.Should().Be(expectedViewModel.IsRegisteredWithOtherAo);
            model.Uln.Should().Be(expectedViewModel.Uln);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationUln);
        }
    }
}
