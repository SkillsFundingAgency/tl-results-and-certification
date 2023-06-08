using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ChangeBackToActiveStatusViewModel _changeBackToActiveStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _changeBackToActiveStatusViewModel = new ChangeBackToActiveStatusViewModel
            {
                ProfileId = ProfileId,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusViewModel>(ProviderUkprn, ProfileId).Returns(_changeBackToActiveStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ChangeBackToActiveStatusViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_changeBackToActiveStatusViewModel.ProfileId);
            model.LearnerName.Should().Be(_changeBackToActiveStatusViewModel.LearnerName);
            model.ChangeBackToActive.Should().BeNull();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Registered_Learners);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            model.Breadcrumb.BreadcrumbItems[1].RouteAttributes.TryGetValue(Constants.AcademicYear, out string academicYearRouteValue);
            academicYearRouteValue.Should().Be(_changeBackToActiveStatusViewModel.AcademicYear.ToString());

            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(Breadcrumb.Learners_Record);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
