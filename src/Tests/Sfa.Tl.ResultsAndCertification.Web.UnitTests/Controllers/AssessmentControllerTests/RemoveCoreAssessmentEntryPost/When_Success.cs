using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_Success : TestSetup
    {
        private bool _response;

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                Uln = 12345678,
                AssessmentId = 5,
                ComponentType = ComponentType.Core,
                CanRemoveAssessmentEntry = true
            };

            _response = true;
            AssessmentLoader.RemoveAssessmentEntryAsync(AoUkprn, ViewModel).Returns(_response);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).RemoveAssessmentEntryAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.RemoveAssessmentEntryConfirmationViewModel),
                Arg.Is<RemoveAssessmentEntryConfirmationViewModel>
                (x => x.ProfileId == ViewModel.ProfileId && x.Uln == ViewModel.Uln), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_RemoveAssessmentEntryConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AssessmentEntryRemovedConfirmation);
        }
    }
}
