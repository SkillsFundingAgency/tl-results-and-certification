using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealGet
{
    public class When_Called_With_Invalid_AppealsEndDate_For_Specialism : TestSetup
    {
        private PrsAddAppealViewModel _addAppealViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addAppealViewModel = new PrsAddAppealViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,                
                ComponentType = ComponentType,
                AppealEndDate = DateTime.UtcNow.AddDays(-7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
