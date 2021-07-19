using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesGet
{
    public class When_Learner_IsNotActive : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearner;
        public override void Given()
        {
            ProfileId = 1;
            _findPrsLearner = new FindPrsLearnerRecord { Status = RegistrationPathwayStatus.Withdrawn };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, null, ProfileId).Returns(_findPrsLearner);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, null, ProfileId);
        }
    }
}
