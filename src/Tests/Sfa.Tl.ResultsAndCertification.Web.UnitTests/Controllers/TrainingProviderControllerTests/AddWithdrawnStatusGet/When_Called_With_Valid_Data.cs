using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddWithdrawnStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddWithdrawnStatusViewModel _addWithdrawnStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addWithdrawnStatusViewModel = new AddWithdrawnStatusViewModel
            {
                ProfileId = ProfileId,
                LearnerName = "test-learner-name",
                RegistrationPathwayStatus = Common.Enum.RegistrationPathwayStatus.Active
            };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddWithdrawnStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addWithdrawnStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddWithdrawnStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AddWithdrawnStatusViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addWithdrawnStatusViewModel.ProfileId);
            model.LearnerName.Should().Be(_addWithdrawnStatusViewModel.LearnerName);
            model.IsPendingWithdrawl.Should().BeNull();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.SearchLearnerRecord);

            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(Breadcrumb.Learners_Record);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
