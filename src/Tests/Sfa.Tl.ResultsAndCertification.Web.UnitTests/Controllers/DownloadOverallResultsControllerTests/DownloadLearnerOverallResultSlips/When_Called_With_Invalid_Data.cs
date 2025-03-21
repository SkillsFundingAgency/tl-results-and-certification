﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadLearnerOverallResultSlips
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            DownloadOverallResultsLoader.DownloadLearnerOverallResultSlipsDataAsync(ProviderUkprn, ProfileId, Email).Returns(null as FileStream);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadLearnerOverallResultSlipsDataAsync(ProviderUkprn, ProfileId, Email);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
