using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class When_Success_No_Tlevels_To_Review : TestSetup
    {
        private readonly int pathwayId = 99;
        private SelectToReviewPageViewModel _mockResult;

        public override void Given()
        {
            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId };
            TlevelLoader.ConfirmTlevelAsync(InputModel).Returns(true);

            _mockResult = new SelectToReviewPageViewModel
            {
                TlevelsToReview = new List<TlevelToReviewViewModel>()
            };

            TlevelLoader.GetTlevelsToReviewByUkprnAsync(AoUkprn).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TlevelLoader.Received(1).ConfirmTlevelAsync(InputModel);
            TlevelLoader.Received(1).GetTlevelsToReviewByUkprnAsync(AoUkprn);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.TlevelConfirmation),
                Arg.Is<bool>(x => x == true), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AllTlevelsReviewedSuccess()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AllTlevelsReviewedSuccess);
        }
    }
}
