using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Xunit;
using EnterUlnContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnterUniqueLearnerReference;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_Uln_IsEmpty : TestSetup
    {
        public override void Given()
        {
            var uln = string.Empty;
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = uln };
            Controller.ModelState.AddModelError("EnterUln", EnterUlnContent.Uln_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(EnterUlnViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(EnterUlnViewModel.EnterUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(EnterUlnViewModel.EnterUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(EnterUlnContent.Uln_Required_Validation_Message);
        }
    }
}
