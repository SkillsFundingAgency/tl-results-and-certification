using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Linq;
using Xunit;
using DateContent = Sfa.Tl.ResultsAndCertification.Web.Content.Helpers.Date;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthPost
{
    public class When_Model_Validated : TestSetup
    {
        public override void Given()
        {
            DateofBirthViewmodel = new DateofBirthViewModel { Day = "d2", Month = "m1", Year = "yy00" };
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(DateofBirthViewModel));

            // Assert Error
            Controller.ModelState.IsValid.Should().Be(false);
            Controller.ModelState.ErrorCount.Should().Be(3);

            var expectedErrorMessage = Controller.ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage;
            expectedErrorMessage.Should().Be(string.Format(DateContent.Validation_Message_Invalid_Date, "Date of birth"));
        }
    }
}
