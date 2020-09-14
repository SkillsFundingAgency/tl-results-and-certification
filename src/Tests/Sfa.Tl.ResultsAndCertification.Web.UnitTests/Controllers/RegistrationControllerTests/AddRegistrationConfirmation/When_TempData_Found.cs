using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationConfirmation
{
    public class When_TempData_Found : TestSetup
    {
        public override void Given()
        {
            RegistrationConfirmationViewModel = new RegistrationConfirmationViewModel { UniqueLearnerNumber = "987654321" };
            CacheService.GetAndRemoveAsync<RegistrationConfirmationViewModel>(Arg.Any<string>()).Returns(RegistrationConfirmationViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationConfirmationViewModel;

            model.Should().NotBeNull();
            model.UniqueLearnerNumber.Should().Be(RegistrationConfirmationViewModel.UniqueLearnerNumber);
        }
    }
}
