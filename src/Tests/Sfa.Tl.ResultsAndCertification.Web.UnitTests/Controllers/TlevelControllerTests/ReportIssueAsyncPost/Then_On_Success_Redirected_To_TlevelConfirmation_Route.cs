﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class Then_On_Success_Redirected_To_TlevelConfirmation_Route : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {
            PathwayId = 99;
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = PathwayId };
            TlevelLoader.ReportIssueAsync(InputViewModel).Returns(true);
        }

        [Fact]
        public void Then_ModelState_Valid_Redirected_To_TlevelConfirmation()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.TlevelConfirmation);
        }
    }
}
