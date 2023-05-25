using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusHaveYouToldAwardingOrganisationGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel _model;

        public override void Given()
        {
            ProfileId = 1;

            _model = new ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel
            {
                ProfileId = ProfileId,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader
                .GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel>(ProviderUkprn, ProfileId)
                .Returns(_model);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel;

            model.Should().NotBeNull();
            model.HaveYouToldAwardingOrganisation.Should().BeNull();
            model.ProfileId.Should().Be(_model.ProfileId);
            model.AwardingOrganisationName.Should().Be(_model.AwardingOrganisationName);
            model.ProviderUkprn.Should().Be(_model.ProviderUkprn);
            model.LearnerName.Should().Be(_model.LearnerName);
            model.AcademicYear.Should().Be(_model.AcademicYear);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Registered_Learners);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            model.Breadcrumb.BreadcrumbItems[1].RouteAttributes.TryGetValue(Constants.AcademicYear, out string academicYearRouteValue);
            academicYearRouteValue.Should().Be(_model.AcademicYear.ToString());

            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(Breadcrumb.Learners_Record);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
