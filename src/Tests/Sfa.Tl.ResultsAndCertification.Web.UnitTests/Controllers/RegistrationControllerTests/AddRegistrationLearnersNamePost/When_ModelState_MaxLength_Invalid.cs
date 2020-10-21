using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using LearnersNameContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.LearnersName;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNamePost
{
    public class When_ModelState_MaxLength_Invalid : TestSetup
    {
        public override void Given()
        {
            LearnersNameViewModel = new LearnersNameViewModel();

            Controller.ModelState.AddModelError("Firstname", LearnersNameContent.Validation_Firstname_Max_Length);
            Controller.ModelState.AddModelError("Lastname", LearnersNameContent.Validation_Lastname_Max_Length);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(LearnersNameViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(LearnersNameViewModel.Firstname)).Should().BeTrue();
            Controller.ViewData.ModelState.ContainsKey(nameof(LearnersNameViewModel.Lastname)).Should().BeTrue();

            var modelStateFirstname = Controller.ViewData.ModelState[nameof(LearnersNameViewModel.Firstname)];
            modelStateFirstname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Firstname_Max_Length);

            var modelStateLastname = Controller.ViewData.ModelState[nameof(LearnersNameViewModel.Lastname)];
            modelStateLastname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Lastname_Max_Length);
        }
    }
}
