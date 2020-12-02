using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesPost
{
    public class When_IsOpted_IsNull : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAssessmentSeriesViewModel 
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = null
            };
        }
        
        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddAssessmentSeriesViewModel));

            var model = viewResult.Model as AddAssessmentSeriesViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AssessmentSeriesId.Should().Be(ViewModel.AssessmentSeriesId);
            model.AssessmentSeriesName.Should().Be(ViewModel.AssessmentSeriesName);
            model.IsOpted.Should().BeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AddAssessmentSeriesViewModel.IsOpted)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AddAssessmentSeriesViewModel.IsOpted)];
            modelState.Errors[0].ErrorMessage.Should().Be($"{AssessmentContent.AddCoreAssessmentEntry.Select_Option_To_Add_Validation_Text} {ViewModel.AssessmentSeriesName}");
        }
    }
}
