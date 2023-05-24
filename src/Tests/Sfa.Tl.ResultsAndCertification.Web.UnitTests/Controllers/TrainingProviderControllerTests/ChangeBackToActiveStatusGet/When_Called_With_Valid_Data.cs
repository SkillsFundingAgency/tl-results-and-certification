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
        private ChangeBackToActiveStatusModel _changeBackToActiveStatusModel;

        public override void Given()
        {
            ProfileId = 1;

            _changeBackToActiveStatusModel = new ChangeBackToActiveStatusModel
            {
                ProfileId = ProfileId,
                LearnerName = "test-learner-name"
            };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusModel>(ProviderUkprn, ProfileId).Returns(_changeBackToActiveStatusModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ChangeBackToActiveStatusModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_changeBackToActiveStatusModel.ProfileId);
            model.LearnerName.Should().Be(_changeBackToActiveStatusModel.LearnerName);
            model.ChangeBackToActive.Should().BeNull();

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
