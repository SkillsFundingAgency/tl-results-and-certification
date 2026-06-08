using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using LearnersNameContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.LearnersName;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNamePost
{
    public class When_ModelState_Invalid_Name_Characters : TestSetup
    {
        public override void Given()
        {
            LearnersNameViewModel = new LearnersNameViewModel();

            Controller.ModelState.AddModelError("Firstname", LearnersNameContent.Validation_Firstname_Cannot_Contain_Integers_Or_Special_Characters);
            Controller.ModelState.AddModelError("Lastname", LearnersNameContent.Validation_Lastname_Cannot_Contain_Integers_Or_Special_Characters);
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
            modelStateFirstname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Firstname_Cannot_Contain_Integers_Or_Special_Characters);

            var modelStateLastname = Controller.ViewData.ModelState[nameof(LearnersNameViewModel.Lastname)];
            modelStateLastname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Lastname_Cannot_Contain_Integers_Or_Special_Characters);
        }
    }
}
