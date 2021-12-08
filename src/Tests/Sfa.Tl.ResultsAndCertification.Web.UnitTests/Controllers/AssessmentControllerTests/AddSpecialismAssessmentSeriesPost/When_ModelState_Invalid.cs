using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = null
            };

            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
            };

            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism, ComponentIds).Returns(_mockresult);
            Controller.ModelState.AddModelError(nameof(AddAssessmentEntryViewModel.IsOpted), AssessmentContent.AddSpecialismAssessmentEntry.Select_Option_To_Add_Validation_Text);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddSpecialismAssessmentEntryViewModel));

            var model = viewResult.Model as AddSpecialismAssessmentEntryViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AssessmentSeriesId.Should().Be(ViewModel.AssessmentSeriesId);
            model.AssessmentSeriesName.Should().Be(ViewModel.AssessmentSeriesName);
            model.IsOpted.Should().BeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AddSpecialismAssessmentEntryViewModel.IsOpted)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AddSpecialismAssessmentEntryViewModel.IsOpted)];
            modelState.Errors[0].ErrorMessage.Should().Be(AssessmentContent.AddSpecialismAssessmentEntry.Select_Option_To_Add_Validation_Text);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(ProfileId.ToString());
        }
    }
}
