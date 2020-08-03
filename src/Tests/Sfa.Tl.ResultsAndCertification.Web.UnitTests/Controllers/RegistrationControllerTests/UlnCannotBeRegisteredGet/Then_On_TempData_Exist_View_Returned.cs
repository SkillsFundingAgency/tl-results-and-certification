using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UlnCannotBeRegisteredGet
{
    public class Then_On_TempData_Exist_View_Returned : When_UlnCannotBeRegistered_Action_Is_Called
    {
        private UlnNotFoundViewModel expectedViewModel;
        public override void Given() 
        {
            expectedViewModel = new UlnNotFoundViewModel();
            CacheService.GetAndRemoveAsync<UlnNotFoundViewModel>(Arg.Any<string>()).Returns(expectedViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UlnNotFoundViewModel;

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
