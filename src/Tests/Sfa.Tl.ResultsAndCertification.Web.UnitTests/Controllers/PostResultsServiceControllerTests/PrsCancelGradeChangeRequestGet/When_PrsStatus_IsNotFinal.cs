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
    public class When_PrsStatus_IsNotFinal : TestSetup
    {
        private PrsCancelGradeChangeRequestViewModel _mockCancelGradeChangeRequestViewModel;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;

            _mockCancelGradeChangeRequestViewModel = new PrsCancelGradeChangeRequestViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Status = RegistrationPathwayStatus.Active,
                PrsStatus = PrsStatus.BeingAppealed,
                AppealEndDate = DateTime.Now.AddDays(1)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_mockCancelGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
