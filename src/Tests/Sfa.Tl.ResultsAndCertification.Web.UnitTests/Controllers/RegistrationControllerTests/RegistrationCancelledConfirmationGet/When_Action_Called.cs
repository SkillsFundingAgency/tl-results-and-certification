using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationCancelledConfirmationGet
{
    public class When_Action_Called : TestSetup
    {
        public override void Given()
        {
            RegistrationCancelledConfirmationViewModel = new RegistrationCancelledConfirmationViewModel { Uln = 987654321 };
            CacheService.GetAndRemoveAsync<RegistrationCancelledConfirmationViewModel>(CacheKey).Returns(RegistrationCancelledConfirmationViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationCancelledConfirmationViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(RegistrationCancelledConfirmationViewModel.Uln);
        }
    }
}
