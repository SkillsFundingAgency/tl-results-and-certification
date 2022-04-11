using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelGradeChangeRequestGet
{
    public class When_IsResultJourney_IsTrue : TestSetup
    {
        private PrsCancelGradeChangeRequestViewModel _mockCancelGradeChangeRequestViewModel;

        public override void Given()
        {
            // Input params
            ProfileId = 11;
            AssessmentId = 1;
            ComponentType = ComponentType.Core;
            IsResultJourney = true;

            _mockCancelGradeChangeRequestViewModel = new PrsCancelGradeChangeRequestViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Status = RegistrationPathwayStatus.Active,
                PrsStatus = PrsStatus.Final,
                AppealEndDate = DateTime.Now.AddDays(1)
            };
            Loader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, (ComponentType)ComponentType).Returns(_mockCancelGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_BackLink_Route_Is_ResultDetails()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsCancelGradeChangeRequestViewModel;

            model.Should().NotBeNull();
            model.IsResultJourney = true;

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsGradeChangeRequest);
            model.BackLink.RouteAttributes.Count.Should().Be(3);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(_mockCancelGradeChangeRequestViewModel.ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(_mockCancelGradeChangeRequestViewModel.AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.IsResultJourney].Should().Be(true.ToString());
        }
    }
}
