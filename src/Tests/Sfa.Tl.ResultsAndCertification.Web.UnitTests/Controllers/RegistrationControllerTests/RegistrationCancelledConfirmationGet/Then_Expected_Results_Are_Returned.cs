using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationCancelledConfirmationGet
{
    public class Then_Expected_Results_Are_Returned : When_RegistrationCancelledConfirmationAsync_Is_Called
    {
        public override void Given()
        {
            RegistrationCancelledConfirmationViewModel = new RegistrationCancelledConfirmationViewModel { Uln = 987654321 };
            CacheService.GetAndRemoveAsync<RegistrationCancelledConfirmationViewModel>(CacheKey).Returns(RegistrationCancelledConfirmationViewModel);
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationCancelledConfirmationViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(RegistrationCancelledConfirmationViewModel.Uln);
        }
    }
}
