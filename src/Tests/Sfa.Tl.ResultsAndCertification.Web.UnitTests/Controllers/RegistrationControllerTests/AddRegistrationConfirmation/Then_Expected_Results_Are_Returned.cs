using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationConfirmation
{
    public class Then_Expected_Results_Are_Returned : When_AddRegistrationConfirmationAsync_Is_Called
    {
        public override void Given()
        {
            RegistrationConfirmationViewModel = new RegistrationConfirmationViewModel { UniqueLearnerNumber = "987654321" };
            TempData[Constants.RegistrationConfirmationViewModel] = JsonConvert.SerializeObject(RegistrationConfirmationViewModel);
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationConfirmationViewModel;

            model.Should().NotBeNull();
            model.UniqueLearnerNumber.Should().Be(RegistrationConfirmationViewModel.UniqueLearnerNumber);
        }
    }
}
