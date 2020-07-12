using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using LearnersNameContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.LearnersName;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNamePost
{
    public class Then_On_ModelState_Invalid_Returns_Required_Validation_Error : When_AddRegistrationLearnersName_Post_Action_Is_Called
    {
        public override void Given()
        {
            LearnersNameViewModel = new LearnersNameViewModel();

            Controller.ModelState.AddModelError("Firstname", LearnersNameContent.Validation_Firstname_Required);
            Controller.ModelState.AddModelError("Lastname", LearnersNameContent.Validation_Lastname_Required);
        }

        [Fact]
        public void Then_Expected_Required_Error_Message_Is_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(LearnersNameViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(LearnersNameViewModel.Firstname)).Should().BeTrue();
            Controller.ViewData.ModelState.ContainsKey(nameof(LearnersNameViewModel.Lastname)).Should().BeTrue();

            var modelStateFirstname = Controller.ViewData.ModelState[nameof(LearnersNameViewModel.Firstname)];
            modelStateFirstname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Firstname_Required);

            var modelStateLastname = Controller.ViewData.ModelState[nameof(LearnersNameViewModel.Lastname)];
            modelStateLastname.Errors[0].ErrorMessage.Should().Be(LearnersNameContent.Validation_Lastname_Required);
        }
    }
}
