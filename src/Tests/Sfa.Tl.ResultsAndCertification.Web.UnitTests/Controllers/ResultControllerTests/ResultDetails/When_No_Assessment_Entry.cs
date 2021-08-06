using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_No_Assessment_Entry : TestSetup
    {
        private ResultDetailsViewModel _mockResult;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel { PathwayAssessmentId = 0 };
            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            CacheService.Received(1).SetAsync(Constants.ResultsSearchCriteria, _mockResult.Uln.ToString());
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<ResultNoAssessmentEntryViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_ResultNoAssessmentEntry()
        {
            var actualRoute = Result as RedirectToRouteResult;
            actualRoute.RouteName.Should().Be(RouteConstants.ResultNoAssessmentEntry);
            actualRoute.RouteValues.Should().BeNull();
        }
    }
}
