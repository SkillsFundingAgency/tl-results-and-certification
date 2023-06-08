using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusHaveYouToldAwardingOrganisationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel
            {
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 1123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            Controller.ModelState.AddModelError(nameof(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel.HaveYouToldAwardingOrganisation), Content.TrainingProvider.ChangeBackToActiveStatusHaveYouToldAwardingOrganisation.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel));

            var model = viewResult.Model as ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel;

            model.Should().NotBeNull();

            model.HaveYouToldAwardingOrganisation.Should().BeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AwardingOrganisationName.Should().Be(ViewModel.AwardingOrganisationName);
            model.ProviderUkprn.Should().Be(ViewModel.ProviderUkprn);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.AcademicYear.Should().Be(ViewModel.AcademicYear);
            
            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel.HaveYouToldAwardingOrganisation)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel.HaveYouToldAwardingOrganisation)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.TrainingProvider.ChangeBackToActiveStatusHaveYouToldAwardingOrganisation.Validation_Message);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Registered_Learners);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            model.Breadcrumb.BreadcrumbItems[1].RouteAttributes.TryGetValue(Constants.AcademicYear, out string academicYearRouteValue);
            academicYearRouteValue.Should().Be(ViewModel.AcademicYear.ToString());

            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(Breadcrumb.Learners_Record);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ViewModel.ProfileId.ToString());
        }
    }
}
